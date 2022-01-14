using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geometery
{
    public class FaceDefinitions : MonoBehaviour
    {
        [Header("Define the block faces")] 
        public SOBrickFaces faces;
        [SerializeField] private bool _regenerateFacesOnLoad = false;
        [SerializeField] private bool _drawDebugLines = false;

        private void Awake()
        {    
            if (_regenerateFacesOnLoad)
            {
                _RegenerateFaces();
            }
        }


        //private void Update()
        //{
        //    if (_drawDebugLines)
        //    {
        //        foreach (var f in faces.faces)
        //        {
        //            Debug.DrawRay(transform.position + f.position, f.normal, Color.red);
        //        }
        //    }
        //}


        //Returns the face nearest to the specified point. The player calls this to get the suggested face for snapping a new block to this one. 
        public BlockFace GetNearestBlockFaceToPoint(Vector3 point)
        {
            float curDistance = 10000f;
            BlockFace curBlockFace = faces.faces[0];
            foreach (var face in faces.faces)
            {
                Vector3 faceCoord = transform.position + face.position;
                float distToFace = Helpers.GetDistanceSquared(point, faceCoord);
                if (distToFace < curDistance)
                {
                    curDistance = distToFace;
                    curBlockFace = face;
                }
            }
            return curBlockFace;
        }


        //Returns the position this block needs to be in to allign with the face of the other block
        public Vector3 GetSnapPositionOffset(BlockFace faceOfOtherBlock)
        {
            //Given the block's current rotation, find the face whose normal vector is opposite the face of the other block we are snapping to
            foreach (var f in faces.faces)
            {
                float angleBetween = Mathf.Abs(Mathf.Abs(Vector3.Angle(f.normal, -faceOfOtherBlock.normal)));
                Debug.LogError(faces.faces.Count);
                if (angleBetween< 5f)
                {
                    Debug.LogError("Found it");
                    return -f.position;
                }
            }
            return Vector3.zero;
        }


        //public Vector3 GetPlacementPositionOffsetForFace(BlockFace face)
        //{
        //    //TODO: for non-squares this will depend on the geometery
        //    return .5f * face.position;
        //}


        //If the geometery chages, we can update the scriptable object this way.
        private void _RegenerateFaces()
        {
            faces.faces = null;
            faces.faces = new List<BlockFace>();

            if (faces.name == "FoundationBrickFaces")
            {                
                float size = 2;
                float dims = 20f;
                float height = .5f;
                float increment = .2f;

                float curX = 0f;
                float curY = 0f;
                for(int i = 0; i < dims; i++)
                {
                    
                    for (int j = 0; j < dims; j++)
                    {
                       
                        faces.faces.Add(new BlockFace
                        {
                            position =  new Vector3(2 -curX - size / dims, height, 2 -curY - size / dims),
                            normal = Vector3.up
                        });
                        curX += increment;
                    }
                    curY += increment;
                    curX = 0f;
                }

            }
        }

    }
}

