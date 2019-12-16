using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private BlackBoard blackBoard;
    private Vector3 targetPos;

    private bool lerpInit = false;
    private float speed = 2.0f;

    private ActionNode searchResouce;
    private ActionNode collectResouce;
    private ActionNode checkBag;

    private ActionNode searchDeposit;
    private ActionNode depositResource;

    private ActionNode moveToPoint;

    private Selector rootNode;
    private Sequence depositeSequence;
    private Sequence collectSequence;

    private Inverter checkBagEmpty;

    public delegate void TreeExecuted();
    public event TreeExecuted onTreeExecuted;

    public delegate void NodePassed(string trigger);


    // Start is called before the first frame update
    void Start(){
        blackBoard = BlackBoard.Instance;


        moveToPoint = new ActionNode(MoveToPosition);

        searchResouce = new ActionNode(SearchResouce);
        collectResouce = new ActionNode(CollectResouce);
        checkBag = new ActionNode(CheckBag);

        checkBagEmpty = new Inverter(checkBag);

        searchDeposit = new ActionNode(SearchDeposit);
        depositResource = new ActionNode(DepositResource);

        depositeSequence = new Sequence(new List<Node>{
            checkBagEmpty,
            searchDeposit,
            moveToPoint,
            depositResource,
        });

        collectSequence = new Sequence(new List<Node>{
            checkBag,
            searchResouce,
            moveToPoint,
            collectResouce,
        });

        rootNode = new Selector(new List<Node>{
            collectSequence,
            depositeSequence,
        });

        Evaluate();

    }

    private void Update() {
        if(lerpInit){
            transform.position = Vector3.Lerp(transform.position,targetPos, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, targetPos) <= 0.1f){
                lerpInit = false;
                transform.position = targetPos;
            }

        }
    }

    public void Evaluate() {
        rootNode.Evaluate();
        StartCoroutine(Execute());
    }

    private IEnumerator Execute() {
        Debug.Log("The AI is thinking...");
        yield return new WaitForSeconds(Time.deltaTime);
        Evaluate();
        if(onTreeExecuted != null) {
            onTreeExecuted();
        }
    }

    private NodeStates SearchResouce() {
        Debug.LogWarning("SearchResouce");
        if(ResourceManager.Instance.HaveResource()){
            blackBoard.resource =  ResourceManager.Instance.GetResource();
            blackBoard.resourcePosition = blackBoard.resource.gameObject.transform.position;
            blackBoard.newPos =  blackBoard.resourcePosition;
            return NodeStates.SUCCESS;
        }else{
            return NodeStates.FAILURE;
        }
    }
    private NodeStates CollectResouce() {
        Debug.LogWarning("CollectResouce");
        if(blackBoard.resource.getResource()){
            GameManager.Instance.SaveResource();
            return NodeStates.SUCCESS;
        }
            return NodeStates.FAILURE;
    }

    private NodeStates SearchDeposit() {
        Debug.LogWarning("SearchDeposit");
        blackBoard.depositPosition =  GameManager.Instance.GetDepositPosition();
        blackBoard.newPos = blackBoard.depositPosition;
        return NodeStates.SUCCESS;
    }

    private NodeStates MoveToPosition(){
        Debug.LogWarning("MoveToPosition");
        if(!lerpInit){
            targetPos = blackBoard.newPos;
            lerpInit = true;
        }
        if(Vector2.Distance(transform.position, targetPos) <= 0.1f){
            lerpInit = true;
            return NodeStates.SUCCESS;
        }
        else
            return NodeStates.FAILURE;

    }


    private NodeStates CheckBag() {
        Debug.LogWarning("CheckBag");
        if(GameManager.Instance.IsBagFull())
            return NodeStates.FAILURE;

            return NodeStates.SUCCESS;
    }

    private NodeStates DepositResource() {
        Debug.LogWarning("DepositResource");
        if(GameManager.Instance.EmptyBag()){
            return NodeStates.FAILURE;
        }else{
            return NodeStates.RUNNING;
        }
    }

}
