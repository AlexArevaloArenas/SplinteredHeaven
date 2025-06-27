using UnityEngine;
using FOVMapping;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace FOVMapping
{
	// Attach this component to 'eyes' of the field of view.
	// Works only when this component is enabled.
	public class FOVAgent : MonoBehaviour
	{
		[Tooltip("Is this agent an eye(set to true for friendly agents and false for hostile agents)?")]
		[SerializeField]
		private bool _contributeToFOV = true;
		public bool contributeToFOV { get => _contributeToFOV; set => _contributeToFOV = value; }

		[Tooltip("How far can an agent see? This value must be equal to or less than the samplingRange of a generated FOV map.")]
		[SerializeField]
		[Range(0.0f, 1000.0f)]
		private float _sightRange = 50.0f;
		public float sightRange { get => _sightRange; set => _sightRange = value; }

		[Tooltip("How widely can an agent see?")]
		[SerializeField]
		[Range(0.0f, 360.0f)]
		private float _sightAngle = 240.0f;
		public float sightAngle { get => _sightAngle; set => _sightAngle = value; }

		[Tooltip("Will this agent disappear if it is in a fog of war(set to true for hostile agents and false for friendly agents)?")]
		[SerializeField]
		private bool _disappearInFOW = false;
		public bool disappearInFOW { get => _disappearInFOW; set => _disappearInFOW = value; }

		[Tooltip("On the boundary of a field of view, if an agent with `disappearInFOW` set to true is under a fog of war whose opacity is larger than this value, the agent disappears.")]
		[SerializeField]
		[Range(0.0f, 1.0f)]
		private float _disappearAlphaThreshold = 0.1f;
		public float disappearAlphaThreshold { get => _disappearAlphaThreshold; set => _disappearAlphaThreshold = value; }
		private bool isUnderFOW = false;
		private List<MeshRenderer> meshRenderers;
		private List<SkinnedMeshRenderer> skinnedMeshRenderers;

		private bool init=false;
        private void Awake()
        {
            if (transform.root.tag == "Enemy")
            {
                _disappearInFOW = true;
                disappearInFOW = true; // Enemy units should not disappear in FOW
                contributeToFOV = false; // Enemy units should not contribute to FOV
				_contributeToFOV = false; // Enemy units should not contribute to FOV
                sightRange = 0; // Enemy units should have a larger sight range

            }
            else
            {
                _disappearInFOW = false; 
				disappearInFOW = false; // Friendly units should disappear in FOW
				contributeToFOV = true; // Friendly units should contribute to FOV
                _contributeToFOV = true; // Friendly units should contribute to FOV
            }
        }
        /*
		private void Start()
		{
			//Init();
            
			skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
			meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();

			if (GetComponent<PartVisualHandler>().linkedPart.owner.obj.tag=="Enemy")
			{
                _disappearInFOW = false; // Enemy units should not disappear in FOW

            }
			else
			{
				_disappearInFOW = true; // Friendly units should disappear in FOW
            }
			
        }
		*/

        public void Init()
		{
            if(init) return; // Prevent re-initialization
			if (disappearInFOW)
			{
				contributeToFOV = false; // If an agent disappears in FOW, it should not contribute to FOV
                _contributeToFOV = false;
            }
			
            if (transform.root.tag == "Enemy")
            {
                _disappearInFOW = true;
                disappearInFOW = true; // Enemy units should not disappear in FOW
                contributeToFOV = false; // Enemy units should not contribute to FOV
                _contributeToFOV = false; // Enemy units should not contribute to FOV
                sightRange = 0; // Enemy units should have a larger sight range

            }
            else
            {
                _disappearInFOW = false; // Friendly units should disappear in FOW
            }
            init = true;
            StartCoroutine(Wait());
        }

		public IEnumerator Wait()
		{
			yield return new WaitForSeconds(0.01f); // Ensure all components are initialized
            skinnedMeshRenderers = GetComponent<PartVisualHandler>().linkedPart.owner.obj.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
            meshRenderers = GetComponent<PartVisualHandler>().linkedPart.owner.obj.GetComponentsInChildren<MeshRenderer>().ToList();
        }

		[HideInInspector]
		public void SetUnderFOW(bool isUnder)
		{
			if (!init)
			{
				return;
			}
			
			isUnderFOW = isUnder;
			if (_disappearInFOW)
			{
				
				if (TryGetComponent(out PartVisualHandler partVisual))
				{
                    if (transform.root.TryGetComponent(out UnitManager unit))
                    {
                        unit.SetHealthbarVisibility(isUnder);
                    }

				}
				


				//if (meshRenderers == null) return;
				if (skinnedMeshRenderers == null) return;

				for (int  i = 0; i < meshRenderers.Count; i++) 
				{
					meshRenderers[i].enabled = isUnder;
				}
				for (int  i = 0; i < skinnedMeshRenderers.Count; i++) 
				{
					if(skinnedMeshRenderers[i] == null) continue;
					skinnedMeshRenderers[i].enabled = isUnder;
				}
			}
		}

		public bool IsUnderFOW()
		{
			return isUnderFOW;
		}

    }
}