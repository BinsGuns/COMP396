﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour{

    private bool mIsOpen = false;

    public  void OnInteract()
    {
        //InteractText = "Press F to ";

        mIsOpen = !mIsOpen;
        //InteractText += mIsOpen ? "to close" : "to open";

        GetComponent<Animator>().SetBool("open", mIsOpen);
    }
}