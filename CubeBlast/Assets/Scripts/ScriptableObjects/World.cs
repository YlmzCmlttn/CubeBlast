using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWorld", menuName = "World")]
public class World : ScriptableObject
{
    // Start is called before the first frame update

    public Level[] levels;

}
