using UnityEditor.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "New Brush", menuName = "Scriptable Object/Tilemap/Brush")]
[CustomGridBrush(hideAssetInstances: false, hideDefaultInstance: true, defaultBrush: false, defaultName: "CustomRandomBrush")]

public class CustomRandomBrush : RandomBrush
{

}
