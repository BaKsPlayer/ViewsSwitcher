using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Custom/Settings")]
public class SettingsObject : ScriptableObject
{
    [SerializeField]
    private string _url;

    [SerializeField]
    private List<BaseView> _views;

    public string Url => _url;
    public List<BaseView> Views => _views;
}