using UnityEngine;
using System.Collections.Generic;
using Pathfinding.Util;

namespace Pathfinding {
	[AddComponentMenu("Pathfinding/Modifiers/Funnel")]
	[System.Serializable]

	public class FunnelModifier : MonoModifier {

		public bool unwrap = true;
		public bool splitAtEveryPortal;

	#if UNITY_EDITOR
		[UnityEditor.MenuItem("CONTEXT/Seeker/Add Funnel Modifier")]
		public static void AddComp (UnityEditor.MenuCommand command) {
			(command.context as Component).gameObject.AddComponent(typeof(FunnelModifier));
		}
	#endif

		public override int Order { get { return 10; } }

		public override void Apply (Path p) {
			if (p.path == null || p.path.Count == 0 || p.vectorPath == null || p.vectorPath.Count == 0) {
				return;
			}

			List<Vector3> funnelPath = ListPool<Vector3>.Claim();

			var parts = Funnel.SplitIntoParts(p);
			for (int i = 0; i < parts.Count; i++) {
				var part = parts[i];
				if (!part.isLink) {
					var portals = Funnel.ConstructFunnelPortals(p.path, part);
					var result = Funnel.Calculate(portals, unwrap, splitAtEveryPortal);
					funnelPath.AddRange(result);
					ListPool<Vector3>.Release(result);
				}
			}
			ListPool<Funnel.PathPart>.Release(parts);
			ListPool<Vector3>.Release(p.vectorPath);
			p.vectorPath = funnelPath;
		}
	}
}
