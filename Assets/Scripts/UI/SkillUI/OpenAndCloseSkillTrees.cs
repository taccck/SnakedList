using UnityEngine;

public class OpenAndCloseSkillTrees : MonoBehaviour
{
    [SerializeField] private GameObject skillTreeButtons;
    [SerializeField] private GameObject snekTree;
    [SerializeField] private GameObject fruitTree;

    public void BackToButtons()
    {
        snekTree.SetActive(false);
        fruitTree.SetActive(false);
        skillTreeButtons.SetActive(true);
    }

    public void OpenSnekTree()
    {
        skillTreeButtons.SetActive(false);
        snekTree.SetActive(true);
    }

    public void OpenFruitTree()
    {
        skillTreeButtons.SetActive(false);
        fruitTree.SetActive(true);
    }
}
