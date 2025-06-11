
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Narrative/Actions/Day transition")]
public class DayTransitionAction : NarrativeAction
{
    public GameObject dayTransitionUI;
    public float transitionDuration = 6f; // Duration of the day transition in seconds

    private NarrativeContext narrativeContext;
    public override void Execute(NarrativeContext context)
    {
        narrativeContext = context;
        narrativeContext.timeManager.StopTime = true;
        GameObject ui = Instantiate(dayTransitionUI, GameObject.FindGameObjectWithTag("Canvas").transform);
        ui.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = (narrativeContext.timeManager.currentDayNumber-1).ToString();
        ui.GetComponent<CanvasGroup>().alpha = 0;
        context.narrativeEventManager.StartCoroutine(ShowDayTransition(ui));
    }

    public IEnumerator ShowDayTransition(GameObject ui)
    {
        while (ui.GetComponent<CanvasGroup>().alpha < 1)
        {
            ui.GetComponent<CanvasGroup>().alpha += 0.01f;
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(transitionDuration/2);
        ui.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = narrativeContext.timeManager.currentDayNumber.ToString();
        yield return new WaitForSeconds(transitionDuration / 2);

        while (ui.GetComponent<CanvasGroup>().alpha > 0)
        {
            ui.GetComponent<CanvasGroup>().alpha -= 0.01f;
            yield return new WaitForSeconds(0.001f);
        }
        narrativeContext.timeManager.StopTime = false;
        Destroy(ui);
    }

}
