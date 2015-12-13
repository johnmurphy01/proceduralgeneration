var	_meshToHide : GameObject;
var	_showTime : float;
var	_hideTime : float;

function OnEnable () {
	_meshToHide.SetActive(false);
}

function AnimationStarted () {
	//Debug.Log("GOT START MSG: " + name);
	yield WaitForSeconds (_showTime);
	_meshToHide.SetActive(true);
	yield WaitForSeconds (_hideTime);
	_meshToHide.SetActive(false);
}

function AnimationEnded () {
	//Debug.Log("GOT END MSG: " + name);
	//yield WaitForSeconds (_hideTime);
	//_meshToHide.SetActiveRecursively(false);
}