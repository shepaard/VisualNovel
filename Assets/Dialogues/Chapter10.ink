VAR charаcterName = "."
VAR characherEmotions = 0

~charаcterName = "Александр"
Друг, слушай …
~charаcterName = "Антон"
~characherEmotions = 4
Давай потом? 
Я пойду к преподше лично. Я ей написал в личном кабинете, но внятного ответа не получил.
~charаcterName = "Александр"
~characherEmotions = 0
Ладно, как скажешь.
~charаcterName = "."
Он в темпе пошел дальше по коридору, активно написывая кому-то сообщение. Я уже было хотел вернутся как передо мной возникла Алиса с нахмуренными бровями.
~charаcterName = "Алиса"
~characherEmotions = 3
Ну что?
	+ [Он пошёл к Анне Николаевне.] -> continue
	+ [Его можно не ждать.] -> continue
	
===continue===
~charаcterName = "."
~characherEmotions = 0
Девушка нервно вздохнула и заглянула за мое плечо, куда убежал Антон.
~charаcterName = "Алиса"
Раздражает меня. 
Ладно, позже разберемся с этим.
~charаcterName = "Александр"
Ты главное не переживай. Все с ним нормально будет.
~charаcterName = "."
Чуть посмеявшись над ее реакцией, мы пошли на следующие пары. 
->END