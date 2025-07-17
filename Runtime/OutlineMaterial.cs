using UnityEngine;

[CreateAssetMenu(fileName = "OutlineMaterial", menuName = "Outline/OutlineMaterial")]
public class OutlineMaterial : ScriptableObject
{
    [SerializeField, Range(1, 60)] 
    private int _spread = 10;

    [SerializeField] 
    private Color _outlineColor = Color.cyan;

    public int Spread => _spread;
    public Color OutlineColor => _outlineColor;

}
