#pragma strict

var timerMax = 0.2;
var timerMin = 2.0;
var onColor : Color = Color(0.2, 0.3, 0.4, 0.5);
var offColor : Color = Color(0.2, 0.3, 0.4, 0.5);

function Start () {
    while (true) {
    	yield WaitForSeconds(Random.Range(timerMin, timerMax));
    	var light : Light = GetComponent(Light);
    	light.enabled = !light.enabled;
    }
}