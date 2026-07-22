extends AnimatedSprite2D

var sfxPlayed = false
var sfx = load("res://assets/sfx/transition.ogg")

func _process(delta: float) -> void:
	if frame == 0:
		stop()
		frame = 1
		CSGlobals.Reset()
	if frame == 1 and !sfxPlayed and is_playing(): playSFX()
	if frame == 15 and !sfxPlayed: playSFX()
	if frame == 25 and $"/root/Instance/InventoryMenu".visible:
		$"/root/Instance/Game/SunPath/Sun".progress_ratio = 0
		$"/root/Instance/InventoryMenu".hide()
		$"/root/Instance/Game".hide()
		$"/root/Instance/Title".show()
		sfxPlayed = false
		
func playSFX():
	$"/root/Audio/SFX".volume_db = 2
	$"/root/Audio/SFX".stream = sfx
	$"/root/Audio/SFX".play()
	$"/root/Audio/SFX".volume_db = 0
	sfxPlayed = true
