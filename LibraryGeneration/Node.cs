using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public string m_name;
    public Vector3 m_position;
    public int m_distanceToNextSibling;

    public bool m_isFirstFolder = false;
    public bool m_isSibling = false;
    public bool m_isParent = false;
    public bool m_isSecondLast = false;
    public bool m_isLast = false;
    public bool m_isRootFolder = false;

    public List<string> listOfFilesNames;

    public Node(string name, Vector3 pos, int distanceToNextSibling, bool isFirstFolder, bool isSibling, bool isParent, bool isSecondLast, bool isLast, bool isRootFolder) 
    {
        m_name = name;
        m_position = pos;
        m_distanceToNextSibling = distanceToNextSibling;
        m_isFirstFolder = isFirstFolder;
        m_isSibling = isSibling;
        m_isParent = isParent;
        m_isSecondLast = isSecondLast;
        m_isLast = isLast;
        m_isRootFolder = isRootFolder;

        listOfFilesNames = new List<string>();
    }

    private void Start()
    {
        this.transform.position = m_position;
        this.name = m_name;
    }

}
