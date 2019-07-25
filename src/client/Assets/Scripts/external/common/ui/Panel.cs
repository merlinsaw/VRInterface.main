using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.Scripts.external.common.UI {
  class Panel : IPanel {

	public VisualElement visualTree => throw new NotImplementedException();

	public EventDispatcher dispatcher => throw new NotImplementedException();

	public ContextType contextType => throw new NotImplementedException();

	public FocusController focusController => throw new NotImplementedException();

	public void Dispose() {
	  throw new NotImplementedException();
	}

	public VisualElement Pick(Vector2 point) {
	  throw new NotImplementedException();
	}

	public VisualElement PickAll(Vector2 point, List<VisualElement> picked) {
	  throw new NotImplementedException();
	}
  }
}
