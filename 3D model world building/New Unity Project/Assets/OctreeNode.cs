using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeNode
{
    Bounds nodeBounds;
    float minSize;
    Bounds[] childBounds;
    public OctreeNode[] children = null;
    public Material materialID;
    public bool mid1 = false;
    public bool dividing = false;

    public OctreeNode(Bounds b, float minNodeSize)
    {
        nodeBounds = b;
        minSize = minNodeSize;

        float quarter = nodeBounds.size.y / 4.0f;
        float childLength = nodeBounds.size.y / 2;
        Vector3 childSize = new Vector3(childLength, childLength, childLength);
        childBounds = new Bounds[8];
        childBounds[0] = new Bounds(nodeBounds.center + new Vector3(-quarter, quarter, -quarter), childSize);
        childBounds[1] = new Bounds(nodeBounds.center + new Vector3(quarter, quarter, -quarter), childSize);
        childBounds[2] = new Bounds(nodeBounds.center + new Vector3(-quarter, quarter, quarter), childSize);
        childBounds[3] = new Bounds(nodeBounds.center + new Vector3(quarter, quarter, quarter), childSize);
        childBounds[4] = new Bounds(nodeBounds.center + new Vector3(-quarter, -quarter, -quarter), childSize);
        childBounds[5] = new Bounds(nodeBounds.center + new Vector3(quarter, -quarter, -quarter), childSize);
        childBounds[6] = new Bounds(nodeBounds.center + new Vector3(-quarter, -quarter, quarter), childSize);
        childBounds[7] = new Bounds(nodeBounds.center + new Vector3(quarter, -quarter, quarter), childSize);
    }

    public void AddObject(GameObject go)
    {
        DivideAndAdd(go);
    }

    public void DivideAndAdd(GameObject go){
        if(nodeBounds.size.y <= minSize){
            // set material ID here since it will be a leaf node 
            if(nodeBounds.Intersects(go.GetComponent<Collider>().bounds)){
                materialID = go.GetComponent<Renderer>().material;
                // if this is null, we will set a bool to true to assign it a default material 
                // during parsing 
                if(materialID==null){
                    mid1 = true; 
                }
                // The only other time materialID should be null is when it is intersecting NOTHING (air)
            }
            return;
        }

        if (children == null)
        {
            children = new OctreeNode[8];
        }

        bool allChildrenSameMaterial = true;
        Material firstChildMaterial = null;

        for (int i = 0; i < 8; i++)
        {
            if (children[i] == null)
            {
                children[i] = new OctreeNode(childBounds[i], minSize);
            }
            if (childBounds[i].Intersects(goCollider.bounds))
            {
                dividing = true;
                children[i].DivideAndAdd(go);
            }

            // Check if all child nodes have the same material
            if (children[i] != null)
            {
                if (i == 0)
                {
                    firstChildMaterial = children[i].materialID;
                }
                else if (children[i].materialID != firstChildMaterial)
                {
                    allChildrenSameMaterial = false;
                }
            }
        }

        // If all children have the same material, set the current node's material and remove children
        if (allChildrenSameMaterial)
        {
            materialID = firstChildMaterial;
            children = null;
            dividing = false;
        }
    }


    public void Draw(){
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireCube(nodeBounds.center, nodeBounds.size);
        if (children != null)
        {
            for (int i = 0; i < 8; i++)
            {
                if (children[i] != null)
                {
                    children[i].Draw();
                }
            }
        }
    }
}
