using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVShadowing : MonoBehaviour
{

    public Color myYellow;
    public Color myOrange;
    public Color myRed;

    private Renderer myRenderer;
    private MaterialPropertyBlock myPropBlock;

    // Start is called before the first frame update
    void Start()
    {
        myPropBlock = new MaterialPropertyBlock();
        myRenderer = GetComponent<Renderer>();

        /*myRenderer.GetPropertyBlock(myPropBlock);
        myPropBlock.SetColor("_BaseColor", myYellow);
        myRenderer.SetPropertyBlock(myPropBlock);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        //GetComponent<Renderer>().material.color = Color.red;

        if (!other.isTrigger) 
        {
            // https://thomasmountainborn.com/2016/05/25/materialpropertyblocks/
        myRenderer.GetPropertyBlock(myPropBlock);
        myPropBlock.SetColor("_UnlitColor", myRed);
        myRenderer.SetPropertyBlock(myPropBlock);
        }
        
    }

    private void OnTriggerStay(Collider other) {
        // Debug.Log(other);
    }

    private void OnTriggerExit(Collider other) {
        if (!other.isTrigger) 
        {
            // https://thomasmountainborn.com/2016/05/25/materialpropertyblocks/
            myRenderer.GetPropertyBlock(myPropBlock);
            myPropBlock.SetColor("_UnlitColor", myYellow);
            myRenderer.SetPropertyBlock(myPropBlock);
        }
    }
}
