
function Drop () {
	GetComponent(Collider).isTrigger = false;
	if (GetComponent(Rigidbody) == null)
		gameObject.AddComponent(Rigidbody);
}