using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTreeCoordinates : MonoBehaviour
{
    [Header("Tree Placement Constraints")]
    [SerializeField] private bool _generateCoordsOnStart;
    [SerializeField] private float _minAlt;
    [SerializeField] private float _minAltFadePoint;
    [SerializeField] private float _maxAlt;
    [SerializeField] private float _maxAltFadePoint;
    [SerializeField] private float _maxSlope;
    [SerializeField] private float _maxSlopeFadePoint;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _spacing;
    [SerializeField] private float _averageOffset;
    [SerializeField] private float _seed;

    [Header("Tree Prefabs To Spawn")]
    [SerializeField] private List<Transform> _treePrefabs;

    [Header("Map Dimensions")]
    [SerializeField] private float _xMin;
    [SerializeField] private float _xMax;
    [SerializeField] private float _zMin;
    [SerializeField] private float _zMax;

    [SerializeField] private Transform _playerReference;
    private List<Transform> _spawnedTrees;

    private float _curX;
    private float _curZ;

    private List<Vector3> _coordinates;

    // Start is called before the first frame update
    void Start()
    {
        if (_generateCoordsOnStart)
        {
            _spawnedTrees = new List<Transform>();
            _coordinates = _GenerateTreeCoordinates();          
        }             
    }

    // Update is called once per frame
    void Update()
    {
        if (_generateCoordsOnStart)
        {
            _DrawTreePosDebugLines();
        }
    }


    private List<Vector3> _GenerateTreeCoordinates()
    {
        List<Vector3> coordinates = new List<Vector3>();
        _curX = _xMin;
        _curZ = _zMin;

        int maxLoopCount = 1000;
        int innerCount = 0; 
        int outerCount = 0;
        while (_curX < _xMax)
        {
            innerCount++;
            _curX += _spacing;
            _curZ = _zMin;
            while (_curZ < _zMax)
            {
                outerCount++;
                _curZ += _spacing;

                Vector3 point = new Vector3(_curX, 1000, _curZ);
                point.z += Random.Range(-_averageOffset, _averageOffset);
                point.x += Random.Range(-_averageOffset, _averageOffset);
                RaycastHit hit;
                if (Physics.Raycast(point, Vector3.down, out hit))
                {
                    if (Vector3.Angle(hit.normal, Vector3.up) > _maxSlope) continue;
                    if (hit.point.y < _maxAlt && hit.point.y > _minAlt)
                    {
                        if (hit.point.y > _maxAltFadePoint)
                        {
                            float percent = (hit.point.y - _maxAltFadePoint) / (_maxAlt - _maxAltFadePoint) * .5f;
                            float randd = Random.Range(0f, 1f);
                            if (randd > percent) continue;
                        }
                        float sample = Mathf.PerlinNoise(point.x  /100f, point.z /100f);
                        float rand = Random.Range(0f, 1f);
                        if(rand > sample)
                        {                            
                            int treeTypeIndex = Random.Range(0, _treePrefabs.Count - 1);
                            Vector3 randRot = new Vector3(0f, rand, 0f);
                            var t = GameObject.Instantiate(_treePrefabs[treeTypeIndex], hit.point, Quaternion.Euler(randRot));
                            float randScale = Random.Range(.9f, 2f);
                            t.localScale = new Vector3(randScale, randScale, randScale);
                            t.tag = "Tree";
                            t.transform.parent = gameObject.transform;
                            t.gameObject.SetActive(false);
                            coordinates.Add(hit.point);
                            _spawnedTrees.Add(t);
                        }
                       
                    }                    
                }
            }
        }
        return coordinates;
    }


    int curIndex = 0;
    private void _DrawTreePosDebugLines()
    {
        for (int i = 0; i < 150; i++)
        {
            curIndex++;
            if (curIndex >= _spawnedTrees.Count - 1) curIndex = 0;
            float dist = _GetDistSquared(_playerReference.position, _spawnedTrees[curIndex].position);
            if (dist > 120000f)
            {
                _spawnedTrees[curIndex].gameObject.SetActive(false);

            }
            else {
                _spawnedTrees[curIndex].gameObject.SetActive(true);
            }
            //if (dist > 1000)
            //{
            //    _spawnedTrees[curIndex].gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            //}
            //else {
            //    _spawnedTrees[curIndex].gameObject.GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            //}
        }
    }

    private float _GetDistSquared(Vector3 v1, Vector3 v2)
    {
        return (v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z);
    }

}
