﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorManager : MBSingleton<SelectorManager>
{
    public Camera mainCamera;

    Element element1 = null;
    Element element2 = null;

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetButtonDown("LeftClick"))
        {
            Element element = CheckHit();

            if (!element || element.elementType == EElement.Ground)
            {
                if (element1) element1 = null;
            }
            else
            {
                element1 = element;
            }
        }

        if (Input.GetButtonDown("RightClick"))
        {
            Element element = CheckHit();

            if (element && element1)
            {
                element2 = element;
            }
        }

        if (element1 && element2)
        {
            element1.ReactOn(element2);
            element2 = null;
        }
    }

    Element CheckHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Element element = hit.transform.GetComponent<Element>();

            if (element)
            {
                if (element.GetComponent<Obstacle>())
                {
                    UIManager.Instance.OnGoalNotOAttainable();
                    return null;
                }

                Ground ground = element.GetComponent<Ground>();
                if (ground)
                    ground.lastPositionClicked = hit.point;

                return element;
            }
        }

        return null;
    }
}
