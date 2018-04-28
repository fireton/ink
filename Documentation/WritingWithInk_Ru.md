# Написание историй на ink

## Введение

**ink** это скриптовый язык, построенный вокруг идеи разметки чистого текста на потоки с тем, чтобы создавать интерактивные скрипты.

На самом базовом уровне он может быть использован для написания историй по типу "выбери себе приключение" или разветвлённого дерева диалогов. Но его реальная сила - в написании диалогов с массой вариантов выбора и большим количеством рекомбинаций потоков текста.

**ink** предлагает несколько функций, позволяющих нетехническим писателям часто разветвлять свою историю и играть с последствиями этих разветвлений, большими и маленькими, и всё это без особых хлопот.

Скрипт создан с целью быть ясным и логически упорядоченным, так чтобы разветвлённый диалог можно было проверить "глазами". Порядок ветвлений описывается в повествовательно манере там, где только это возможно.

Также **ink** задумывался с учётом переписывания историй, так что правка должна быть простой и быстрой.

# Часть первая: Основы

## 1) Содержимое

### Простейший ink-скрипт

Самый базовый ink-скрипт - это просто текст в файле с расширением .ink:

	Привет, мир!

Если запустить его, он выведет своё содержимое, а затем остановится.

Текст, разбитый на строки, создаёт новые параграфы. Такой скрипт:

	Привет, мир!
	Привет?
	Привет, ты там?

выведет результат, выглядящий точно так же.


### Комментарии

По умолчанию, весь текст в вашем файле будет выведен в результат, если только он не размечен специальным образом.

Простейшей разметкой являются комментарии. **ink** поддерживает два типа комментариев. Первый тип используется для того, кто читает код, компилятор игнорирует такие комментарии:

	"Что вы думаете об этом?", спросила она.

	// Нечто непечатное...

	"Я никак не могу это прокомментировать",  ответил я.

	/*
		... или неограниченный кусок текста
	*/

И существует другой тип комментариев, служащий для того, чтобы напомнить автору что ему нужно сделать. Компилятор выводит такие комментарии в процессе компиляции:

	TODO: Эту часть следует переписать!

### Теги

Текстовое содержимое игры появится на выходе "как есть", когда движок будет запущен. Однако, иногда бывает полезно дополнить строку содержимого дополнительной информацией о том, чтобы сказать игре, что делать с этим содержимым.

**ink** предоставляет простую систему a simple system пометки строк содержимого с помощью хэштегов.

	=== содержимое
		Строка нормального текста из игры. # покрасить в синий

Эти хэштеги не попадают в основной поток текста, но могут быть прочитаны игрой и использованы так, как вы желаете. См.  [RunningYourInk](https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md#marking-up-your-ink-content-with-tags) для более подробной информации.


## 2) Варианты выбора

Игрок взаимодействует с игрой посредством выбора из вариантов. Текстовый вариант выбора обозначается символом `*`.

Если никаких других инструкций не указано, когда игрок делает выбор, его текст попадает в основной текст игры.

	Привет, мир.
	*	И тебе привет!
		Рад тебя видеть!

Результатом будет следующая игра:

	Привет, мир.
	1: И тебе привет!

	> 1
	И тебе привет!
	Рад тебя видеть!

По умолчанию, текст выбора появляется снова, в выходном потоке текста.

### Подавление текста выбора

Некоторые игры разделяют текст варианта и результат, получаемый при его выборе. В **ink** если текст выбора записан в квадратных скобках, то он не будет напечатан в ответе.

	Привет, мир!
	*	[И тебе привет!]
		Приятно слышать!

выводит

	Привет, мир!
	1: И тебе привет!

	> 1
	Приятно слышать!

#### Дополнительно: текст выбора и выводимый текст можно смешивать

На самом деле, квадратные скобки разделяют содержимое варианта выбора. То, что до них выводится и в тексте выбора, и при ответе; то, что внутри - только в тексте выбора; и то, что после - только в тексте ответа. В сущности, они предоставляют альтернативные способы описания строки текста.

	Привет, мир!
	*	И тебе [привет!] тоже привет, дружище!
		Приятно слышать!

выводит:

	Привет, мир!
	1: И тебе привет!
	> 1
	И тебе тоже привет, дружище!
	Приятно слышать!

Это особенно удобно при написании вариантов в диалогах:

	"В чём дело?" - спросил мой господин.
	*	"Я несколько устал[."]", повторил я.
		"В самом деле" - сказал он. "Как разрушительно."

выводит:

	"В чём дело?" - спросил мой господин.
	1. "Я несколько устал."
	> 1
	"Я несколько устал", повторил я.
	"В самом деле" - сказал он. "Как разрушительно."

### Множественный выбор

Чтобы сделать выбор действительно выбором, необходимо предоставить несколько вариантов для выбора. Мы можем сделать это попросту добавив их в список:

	"В чём дело?" - спросил мой господин.
	*	"Я несколько устал[."]", повторил я.
		"В самом деле" - сказал он. "Как разрушительно."
	*	"Ни в чём, месье!"[] - ответил я.
		"Что ж, очень хорошо."
	*	"Я говорю, это путешествие отвратительно[."] и я не хочу больше в нём участвовать."
		"А", ответил он без раздражения. "Я вижу, ты раздосадован. Завтра всё наладится."

На выходе получится следующая игра:

	"В чём дело?" - спросил мой господин.

	1: "Я несколько устал."
	2: "Ни в чём, месье!"
	3: "Я говорю, это путешествие отвратительно."

	> 3
	"Я говорю, это путешествие отвратительно и я не хочу больше в нём участвовать."
	"А", ответил он без раздражения. "Я вижу, ты раздосадован. Завтра всё наладится."

Синтаксис в примере выше достаточен, чтобы написать единичный набор вариантов выбора. В настоящей игре нам нужно перемещаться с одной точки на другую, основываясь на выборе игрока. Чтобы достичь этого, необходимо добавить ещё немного структуры.

## 3) Узлы

### Части содержимого называются узлами

Чтобы позволить игре разветвляться, нам нужно пометить секции содержимого, дав им названия (как в старых книгах-играх с их "Параграф 18" и тому подобное).

Эти секции называются "узлами" и они являются фундаментальным элементом структуры в **ink**.

### Написание узла

Начало узла обозначается двумя или более знаками равенства, как показано дальше:

	=== верхний_узел ===

(Знаки "равно" в конце необязательны; и название должно быть единым словом без пробелов.)

Начало узла - это заголовок, дальнейший текст попадает внутрь этого узла.

	=== назад_в_лондон ===

	Мы прибыли в Лондон ровно в 9:45 вечера.

#### Дополнительно: более узловатый "привет мир"

Когда вы начинаете писать файл ink, содержимое вне узлов будет запущено автоматически. Но не узлы. Так что если вы начинаете использовать узлы для разметки вашего текста, вам следует объяснить игре, куда идти. Мы сделаем это с помощью стрелки перехода `->`, о которой подробнее поговорим в следующем разделе.

Простейший скрипт с узлами выглядит так:

	-> верхний_узел

	=== верхний_узел ===
	Привет, мир!

Однако, **ink** не любит неоконченные дела и выдаёт предупреждение при компиляции и/или во время запуска, когда думает, что это произошло. Скрипт выше выдаст вот такое при компиляции:

	WARNING: Apparent loose end exists where the flow runs out. Do you need a '-> END' statement, choice or divert? on line 3 of tests/test.ink

	ВНИМАНИЕ: Обнаружен незакрытый финал, поток просто прерывается. Возможно нужен '-> END', выбор или переход? на строке 3 в файле tests/test.ink

и вот такое при запуске:

	Runtime error in tests/test.ink line 3: ran out of content. Do you need a '-> DONE' or '-> END'?

	Ошибка выполнения tests/test.ink line 3: закончилось содержимое. Возможно, нужен '-> DONE' или '-> END'?

Следующий скрипт компилируется и запускается без ошибок:

	=== верхний_узел ===
	Привет, мир!
	-> END

`-> END` является маркером и для писателя, и для компилятора; он означает "течение истории должно теперь прекратиться".

## 4) Переходы

### Узлы направляют в другие узлы

Вы можете приказать истории двигаться от узла к узлу, используя `->`, "стрелку перехода". Переход происходит немедленно, безо всякого вмешательства игрока.

	=== назад_в_лондон ===

	Мы прибыли в Лондон ровно в 9:45 вечера.
	-> спешить_домой

	=== спешить_домой ===
	Мы поспешили домой на Савил-Роу так быстро, как только могли.

#### Перенаправления невидимы

Переходы специально сделаны бесшовными и могут происходить даже посреди предложения:

	=== спешить_домой ===
	WМы поспешили домой на Савил-Роу так быстро -> как_только_могли

	=== как_только_могли ===
	, как только могли.

выдаст тот же текст, что и выше:

	Мы поспешили домой на Савил-Роу так быстро, как только могли.

#### Клей

По умолчанию, **ink** вставляет разрыв строки перед каждой новой строкой текста. В некоторых случаях, однако, необходимо настоять, чтобы в тексте не было разрывов строк, и это можно сделать с помощью `<>`, или "клея".

	=== спешить_домой ===
	Мы поспешили домой <>
	-> на_савил_роу

	=== на_савил_роу ===
	на Савил-Роу
	-> быстро_как_только_могли

	=== быстро_как_только_могли ===
	<> так быстро, как только могли.

тоже выводит:

	Мы поспешили домой на Савил-Роу так быстро, как только могли.

Вы не можете намазать слишком много клея: несколько "клеев", идущих подряд, не имеют дополнительного эффекта. (Также, нет способа "нейтрализовать" клей; как только строка стала липкой, она прилипнет.)


## 5) Ветвление истории

### Базовое ветвление

Комбинируя узлы, варианты выбора и переходы, мы получим базовую структуру игры типа "выбери себе приключение".

	=== параграф_1 ===
	Ты стоишь перед стеной, окружающей Аналанд, с мечом в руке.
	* [Открыть ворота] -> параграф_2
	* [Разбить ворота] -> параграф_3
	* [Развернуться и пойти домой] -> параграф_4

	=== параграф_2 ===
	Ты открываешь ворота и шагаешь по дороге.

	...

### Ветвление и соединение

Используя переходы, писатель может разветвлять повествование и соединять его обратно, не показывая при этом игроку, что оно было заново слито в единый поток.

	=== назад_в_лондон ===

	Мы прибыли в Лондон ровно в 9:45 вечера.

	*	"Мы не должны проиграть!"[], провозгласил я.
		-> спешить_наружу

	*	"Месье, давайте насладимся этим мгновением!"[], воскликнул я.
		Господин дал мне подзатыльник и выволок меня через дверь наружу.
		-> вытащен_наружу

	*	[Мы поспешили домой] -> спешить_наружу


	=== спешить_наружу ===
	Мы поспешили домой на Савил-Роу
	-> быстро_как_только_могли


	=== вытащен_наружу ===
	Он настоял, чтобы мы поспешили домой на Савил-Роу
	-> быстро_как_только_могли


	=== быстро_как_только_могли ===
	<> так быстро, как только могли.


### Повествование

Узлы и переходы комбинируются, чтобы создать базовое повествование в игре. Это повествование "плоское" - в нём нет "стека вызовов" и из переходов нельзя "вернуться".

В большинстве ink-скриптов история начинается сверху, мечется по всему коду в стиле спагетти и, в конце концов, будем надеяться, достигает `-> END`.

Очень свободная структура означает, что писатель может легко взяться за дело, разветвляя и объединяя повествование не волнуясь о структуре, которую создаёт прямо на ходу. Нет каких-то шаблонных заготовок для создания новых ветвей или переходов, а также нет необходимости следить за текущим состоянием.

#### Дополнительно: Циклы

Вы безусловно можете использовать переходы для создания зацикленного содержимого, и в **ink** есть несколько функциональных возможностей для их использования, включая такие, которые помогут тексту меняться самостоятельно, а также способы контролировать как часто может быть выбран тот или иной вариант выбора.

Смотрите главы *Меняющийся текст* и *Условные варианты выбора* для более подробной информации.

О, и следующее вполне легально, но не слишком хорошая идея:

	=== кругом ===
	и
	-> кругом

## 6) Включения и стежки

### Узлы можно разделять на более мелкие части

По мере того, как история становится длиннее, её всё труднее сохранять в организованном виде без какой-либо дополнительной структуры.

Узлы могут включать в себя подсекции, которые называются "стежки". Они размечаются с использованием единичного знака равенства.

	=== восточный_экспресс ===
	= в_первом_классе
		...
	= в_третьем_классе
		...
	= в_вагоне_охраны
		...
	= опоздали_на_поезд
		...

Можно, например, использовать узел в качестве сцены, а стежки - для событий на этой сцене.

### У стежков уникальные названия

На стежок можно перейти, используя его "адрес".

	*	[Путешествовать третьим классом]
		-> восточный_экспресс.в_третьем_классе

	*	[Путешествовать в вагоне охраны]
		-> восточный_экспресс.в_вагоне_охраны

### Первый стежок используется по умолчанию

Переход на узел, содержащий стежки, идёт на первый стежок в этом узле. Так:

	*	[Путешествовать первым классом]
		"Первый класс, месье. Где же ещё?"
		-> восточный_экспресс

то же самое, что и:

	*	[Путешествовать первым классом]
		"Первый класс, месье. Где же ещё?"
		-> восточный_экспресс.в_первом_классе

(...если только мы не изменили порядок стежков в узле!)

Вы также можете добавить текст на вершине узла, вне любого из стежков. Однако вам нужно помнить, что потребуется переход - **ink** *не* заходит автоматически в первый стежок после того, как отработал текст в "заголовке".

	=== восточный_экспресс ===

	Мы сели в поезд, но куда?
	*	[Первый класс] -> в_первом_классе
	*	[Второй класс] -> во_втором_классе

	= в_первом_классе
		...
	= во_втором_классе
		...


### Локальные переходы

Внутри узла нет необходимости использовать полный адрес стежка.

	-> восточный_экспресс

	=== восточный_экспресс ===
	= в_первом_классе
		Я устроил господина здесь.
		*	[Перейти в третий класс]
			-> в_третьем_классе

	= в_третьем_классе
		Себя я поместил в третий.

Это означает, что у узлов и стежков не может быть одинаковых имён, но несколько узлов могут содержать стежки с одинаковыми именами. (Так и в Восточном Экспрессе, и на пароходе "Монголия" может быть свой первый класс.)

Компилятор предупредит вас, если используются неоднозначные названия.

### Файлы скриптов можно комбинировать

Вы также можете разделить содержимое игры по нескольким файлам, используя команду включения `INCLUDE`.

	INCLUDE newspaper.ink
	INCLUDE cities/vienna.ink
	INCLUDE journeys/orient_express.ink


Команды `INCLUDE` всегда должны располагаться в начале файла, не внутри узлов.

Не существует правила о том, в каком файле должен быть узел, чтобы на него можно было перейти. (Другими словами, разделение кода игры по файлам не влияет на пространства имён в игре.)

## 5) Меняющиеся варианты выбора

### Вариант может быть выбран лишь единожды

По умолчанию, каждый вариант выбора в игре может быть выбран лишь один раз. Если у вас нет циклов в вашей истории, то вы никогда не заметите этого поведения. Но если циклы присутствуют, вы быстро обнаружите, что варианты выбора исчезают...

	=== найти_помощь ===

		Вы отчаянно ищете дружественное лицо в толпе.
		*	Женщина в шляпе[?] резко отталкивает вас. -> найти_помощь
		*	Мужчина с портфелем[?] выглядит недовольным, когда вы спотыкаетесь о него. -> найти_помощь

приводит к:

	Вы отчаянно ищете дружественное лицо в толпе.

	1: Женщина в шляпе?
	2: Мужчина с портфелем?

	> 1
	Женщина в шляпе резко отталкивает вас.
	Вы отчаянно ищете дружественное лицо в толпе.

	1: Мужчина с портфелем?

	>

... и на следующем витке цикла у вас не останется варианта выбора.

#### Запасной вариант

Приведённый выше пример останавливается там, где показано, потому что следующий выбор заканчивается ошибкой времени выполнения "out of content" ("закончилось содержимое").

	> 1
	Мужчина с портфелем выглядит недовольным, когда вы спотыкаетесь о него.
	Вы отчаянно ищете дружественное лицо в толпе.

	Runtime error in tests/test.ink line 6: ran out of content. Do you need a '-> DONE' or '-> END'?

	Ошибка выполнения tests/test.ink line 6: закончилось содержимое. Возможно, нужен '-> DONE' или '-> END'?

Мы можем решить эту проблему с помощью "запасного варианта". Запасные варианты никогда не показываются игроку, но они "выбираются" игрой тогда, когда никаких других вариантов не осталось.

Запасной вариант - это попросту "вариант баз текста выбора":
A fallback choice is simply a "choice without choice text":

	*	-> варианты_кончились

И, с небольшим издевательством над синтаксисом, мы можем сделать вариант по умолчанию с содержимым, используя "выбери тогда" стрелку:

	* 	->
		Малдер так никогда и не смог объяснить, как он тогда выбрался из горящей машины. -> второй_сезон

#### Пример запасного варианта

Добавим всё это в предыдущий пример и получим:

	=== найти_помощь ===

		Вы отчаянно ищете дружественное лицо в толпе.
		*	Женщина в шляпе[?] резко отталкивает вас. -> найти_помощь
		*	Мужчина с портфелем[?] выглядит недовольным, когда вы спотыкаетесь о него. -> найти_помощь
		*	->
			Но слишком поздно: вы падаете на вокзальную платформу. Это конец.
			-> END

что выдаст:

	Вы отчаянно ищете дружественное лицо в толпе.

	1: Женщина в шляпе?
	2: Мужчина с портфелем?

	> 1
	Женщина в шляпе резко отталкивает вас.
	Вы отчаянно ищете дружественное лицо в толпе.

	1: Мужчина с портфелем?

	> 1
	Мужчина с портфелем выглядит недовольным, когда вы спотыкаетесь о него.
	Вы отчаянно ищете дружественное лицо в толпе.
	Но слишком поздно: вы падаете на вокзальную платформу. Это конец.


### Липкие варианты

Поведение "только раз", конечно же, не всегда бывает желаемым. Так что нам нужен ещё один тип варианта выбора: "липкий". Липкий вариант просто не исчезает после выбора, и его можно создать с помощью знака `+` (вместо `*`) в начале.

	=== диван_гомера ===
		+	[Съесть ещё пончик]
			Ты ешь ещё один пончик. -> диван_гомера
		*	[Встать с дивана]
			Ты с трудом поднимаешься с дивана, чтобы пойти и написать эпичное стихотворение.
			-> END

Запасные варианты тоже могут быть липкими.

	=== цикл_беседы
		*	[Поговорить о погоде] -> обсуждение_погоды
		*	[Поговорить о детях] -> обсуждение_детей
		+	-> посидеть_ещё_молча

### Условные варианты выбора

Вы также можете вручную включать и выключать варианты. **ink** имеет довольно много встроенной логики, но легче всего проверить условие "видел ли игрок конкретную часть содержимого игры".

Каждый узел/стежок в игре имеет уникальный адрес (чтобы на него можно было перейти), и мы можем использовать тот же адрес, чтобы проверить видел ли игрок эту часть повествования.

	*		{not был_в_париже} 	[Ехать в Париж] -> был_в_париже
	+		{был_в_париже}	 	[Назад в Париж] -> был_в_париже

	*		{был_в_париже.встретил_эстель} [Позвонить мадам Эстель] -> звонок_эстель

Заметьте, что проверка `имя_узла` проходит, если *любой* из стежков этого узла был посещён ранее.

Также имейте в виду, что условия не перекрывают поведение "можно выбрать единожды", вам всё равно нужна липкость для возможности повторного выбора.

#### Дополнительно: множественные условия

Вы можете использовать несколько проверок для варианта выбора. В этом случае, чтобы вариант появился, необходимо, чтобы *все* проверки были пройдены успешно.

	*	{ not был_в_париже } 	[Ехать в Париж] -> был_в_париже
	+	{ был_в_париже } {not устал_от_парижа} [Вернуться в Париж] -> был_в_париже


#### Дополнительно: метки узла/стежка на самом деле счётчики количества посещений

Проверка:

	*	{видел_подсказку} [Обвинить мистера Джефферсона]

на самом деле проверяет *число*, а не флажок правда/ложь. Узел или стежок, использованный таким образом, в действительности - целочисленная переменная, содержащая количество раз, которые игрок видел содержимое по этому адресу.

Если оно не равно нулю, оно вернёт *истина* в такой проверке, как написана выше. Но проверка также может быть более конкретной:

	* {видел_подсказку > 3} [Немедленно арестовать мистера Джефферсона]

#### Дополнительно: больше логики

**ink** поддерживает гораздо более сложную логику и условия, чем описанные тут - смотри главу *Переменные и логика*.


## 6) Изменяющийся текст

### Текст может варьироваться

До сих пор всё содержимое игры, что мы видели, было статичным, фиксированные куски текста. Но содержимое также может меняться, прямо в момент вывода.

### Последовательности, циклы и другие списки

Простейший способ варьировать текст - с помощью списков, из которых он выбирается в соответствии с некоторыми правилами. **ink** поддерживает несколько видов. Списки записываются внутри фигурных скобок `{` и `}`, а элементы отделены друг от друга вертикальной чертой `|`.

Естественно, что они полезны лишь в таких местах, которые посещаются больше одного раза!

#### Типы списков

**Последовательности** (по умолчанию):

Последовательность (или "останавливающий список") это список, который следит, сколько раз его посещали, и каждый раз показывает следующий элемент. Когда элементы списка заканчиваются, он продолжает показывать последний из них.

	Радио с шипением ожило. {"Три!"|"Два!"|"Один!"|Было слышно гудение белого шума от взрыва.|Но это был просто шум помех.}

	{Я купил кофе, заплатив пятифунтовой купюрой.|Я купил второй кофе для друга.|У меня больше не было денег на кофе.}

**Циклы** (отмечаются символом `&`):

Циклы похожи на последовательности, но они повторяют содержимое по кругу.

	Сегодня {&понедельник|вторник|среда|четверг|пятница|суббота|воскресенье}.


**"Одноразовые"** (отмечаются символом `!`):

Одноразовые списки похожи на последовательности, но когда у них заканчиваются элементы, они просто ничего не выводят. (Можете думать о них, как о последовательностях с пустым последним элементом.)

	Он рассказал мне анекдот. {!Я вежливо посмеялся.|Я улыбнулся.|Я поморщился.|Я пообещал себе больше не реагировать.}

**Перетасовки** (отмечаются символом `~`):

Перетасовки выдают случайный результат.

	Я подбросил монетку. {~Орёл|Решка}.

#### Возможности списков

Списки могут содержать пустые элементы.

	Я шагнул вперёд. {!||||Неожиданно погас свет. -> ой}

Списки могут быть вложенными.

	Крысомедведь{&{, не тратя время попусту, | }ударил|царапнул} {&тебя|тебе прямо по {&ноге|руке|щеке}}.

Списки могут включать переходы.

	I {подождал.|ещё подождал.|вздремнул.|проснулся и подождал ещё.|сдался и ушёл. -> уйти_из_почты}

Также их можно использовать внутри текста выбора:

	+ 	"Приветствую, {&господин|месье Фогг|вас|кареглазка}!"[], воскликнул я.

(...с одним предостережением; вы не можете начать текст выбора с `{`, потому что это будет понято как условие.)

#### Примеры

Списки можно использовать внутри циклов чтобы без больших усилий создавать видимость умного, управляющего своим состоянием геймплея.

Вот одноузловая версия игры "Прибей крота". Обратите внимание, мы используем одноразовые варианты выбора и запасной вариант, чтобы гарантировать, что крот не будет бегать по кругу, а игра обязательно завершится.

	=== прибей_крота ===
		{Я взвесил в руке молоток.|{~Мимо!|Пусто!|Неудача. Где же он?|Ага! Попался! -> END}}
		{&Крот|{&Мерзкий|Проклятый|Грязный} {&уродец|грызун}} {где-то здесь|где-то прячется|всё ещё не пойман|смеётся надо мной|всё ещё не пристукнут|обречён}. <>
		{!Я ему покажу!|В этот раз ему не скрыться!}
		*  [{&Ударить|Шмякнуть|Попробовать} верхний левый] 	-> прибей_крота
		*  [{&Врезать в|Засадить в|Треснуть} верхний правый] -> прибей_крота
		*  [{&Хряснуть|Обрушиться на} середину] -> прибей_крота
		*  [{&Залепить в|Бухнуть} нижний левый] 	-> прибей_крота
		*  [{&Заехать в|Шибануть} нижний правый] 	-> прибей_крота
		*  [] А потом ты свалился от голода. Крот победил тебя!
			-> END

в результате получим такую "игру":

	Я взвесил в руке молоток.
	Крот где-то здесь. Я ему покажу!

	1: Ударить верхний левый
	2: Врезать в верхний правый
	3: Хряснуть середину
	4: Залепить в нижний левый
	5: Заехать в нижний правый

	> 1
	Мимо!
	Мерзкий уродец где-то прячется. В этот раз ему не скрыться!

	1: Засадить в верхний правый
	2: Обрушиться на середину
	3: Бухнуть нижний левый
	4: Шибануть нижний правый

	> 4
	Пусто!

	Крот всё ещё не пойман.
	1: Треснуть верхний правый
	2: Хряснуть середину
	3: Залепить в нижний левый

	> 2
	Где же он?

	Проклятый грызун смеётся надо мной
	1: Врезать в верхний правый
	2: Бухнуть нижний левый

	> 1
	Ага! Попался!


А вот небольшой совет по образу жизни. Обратите внимание на липкий вариант выбора - телевиденье всегда соблазнительно:

	=== включить_телевизор ===
	Я {в первый раз|во второй раз|опять|ещё раз} включил телевизор, но там {не было ничего интересного, так что я снова выключил его|всё ещё не было ничего достойного|было ещё меньше интересного, чем раньше|показывали всякую фигню|была програма про акул, а я не люблю акул|ничего не было}.
	+	[Попытаться ещё раз]	-> включить_телевизор
	*	[Пойти прогуляться]		-> пойти_прогуляться

    === пойти_прогуляться ===
    -> END



#### Дополнительно: Многострочные списки
Также, в **ink** есть другой формат описания списков для изменения блоков текста. Смотри главу "многострочные блоки" для более подробной информации.

### Условный текст

Текст может также меняться на основания логических проверок, точно также, как и варианты выбора.

	{встретил_блофельда: "Я видел его. Лишь на мгновение." }

и
	"Его настоящее имя {встретил_блофельда.узнал_его_имя: Франц|осталось в секрете}."

Такие условия могут располагаться на разных строках или внутри одной части текста. Они даже могут быть вложены друг в друга:

	{встретил_блофельда: "Я видел его. Лишь на мгновение. Его настоящее имя {встретил_блофельда.узнал_его_имя: Франц|осталось в секрете}." | "Я упустил его. Это какой-то особенно злой человек?" }

может выдать либо:

	"Я видел его. Лишь на мгновение. Его настоящее имя Франц."

или:

	"Я видел его. Лишь на мгновение. Его настоящее имя осталось в секрете."

или же:

	"Я упустил его. Это какой-то особенно злой человек?"

## 7) Game Queries

**ink** provides a few useful 'game level' queries about game state, for use in conditional logic. They're not quite parts of the language, but they're always available, and they can't be edited by the author. In a sense, they're the "standard library functions" of the language.

The convention is to name these in capital letters.

### CHOICE_COUNT

`CHOICE_COUNT` returns the number of options created so far in the current chunk. So for instance.

	*	{false} Option A
	* 	{true} Option B
	*  {CHOICE_COUNT() == 1} Option C

produces two options, B and C. This can be useful for controlling how many options a player gets on a turn.

### TURNS_SINCE

`TURNS_SINCE` returns the number of moves (formally, player inputs) since a particular knot/stitch was last visited.

A value of 0 means "was seen as part of the current chunk". A value of -1 means "has never been seen". Any other positive value means it has been seen that many turns ago.

	*	{TURNS_SINCE(-> sleeping.intro) > 10} You are feeling tired... -> sleeping
	* 	{TURNS_SINCE(-> laugh) == 0}  You try to stop laughing.

Note that the parameter passed to `TURNS_SINCE` is a "divert target", not simply the knot address itself (because the knot address is a number - the read count - not a location in the story...)

TODO: (requirement of passing `-c` to the compiler)

#### Дополнительно: more queries

You can make your own external functions, though the syntax is a bit different: see the section on functions below.


# Part 2: Weave

So far, we've been building branched stories in the simplest way, with "options" that link to "pages".

But this requires us to uniquely name every destination in the story, which can slow down writing and discourage minor branching.

**ink** has a much more powerful syntax available, designed for simplifying story flows which have an always-forwards direction (as most stories do, and most computer programs don't).

This format is called "weave", and its built out of the basic content/option syntax with two new features: the gather mark, `-`, and the nesting of choices and gathers.

## 1) Gathers

### Gather points gather the flow back together

Let's go back to the first multi-choice example at the top of this document.

	"What's that?" my master asked.
		*	"I am somewhat tired[."]," I repeated.
			"Really," he responded. "How deleterious."
		*	"Nothing, Monsieur!"[] I replied.
		*  "I said, this journey is appalling[."] and I want no more of it."
			"Ah," he replied, not unkindly. "I see you are feeling frustrated. Tomorrow, things will improve."

In a real game, all three of these options might well lead to the same conclusion - Monsieur Fogg leaves the room. We can do this using a gather, without the need to create any new knots, or add any diverts.

	"What's that?" my master asked.
		*	"I am somewhat tired[."]," I repeated.
			"Really," he responded. "How deleterious."
		*	"Nothing, Monsieur!"[] I replied.
			"Very good, then."
		*  "I said, this journey is appalling[."] and I want no more of it."
		"Ah," he replied, not unkindly. "I see you are feeling frustrated. Tomorrow, things will improve."

	-	With that Monsieur Fogg left the room.

This produces the following playthrough:

	"What's that?" my master asked.

	1: "I am somewhat tired."
	2: "Nothing, Monsieur!"
	3: "I said, this journey is appalling."

	> 1
	"I am somewhat tired," I repeated.
	"Really," he responded. "How deleterious."
	With that Monsieur Fogg left the room.

### Options and gathers form chains of content

We can string these gather-and-branch sections together to make branchy sequences that always run forwards.

	=== escape ===
	I ran through the forest, the dogs snapping at my heels.

		* 	I checked the jewels[] were still in my pocket, and the feel of them brought a spring to my step. <>

		*  I did not pause for breath[] but kept on running. <>

		*	I cheered with joy. <>

	- 	The road could not be much further! Mackie would have the engine running, and then I'd be safe.

		*	I reached the road and looked about[]. And would you believe it?
		* 	I should interrupt to say Mackie is normally very reliable[]. He's never once let me down. Or rather, never once, previously to that night.

	-	The road was empty. Mackie was nowhere to be seen.

This is the most basic kind of weave. The rest of this section details  additional features that allow weaves to nest, contain side-tracks and diversions, divert within themselves, and above all, reference earlier choices to influence later ones.

#### The weave philosophy

Weaves are more than just a convenient encapsulation of branching flow; they're also a way to author more robust content. The `escape` example above has already four possible routes through, and a more complex sequence might have lots and lots more. Using normal diverts, one has to check the links by chasing the diverts from point to point and it's easy for errors to creep in.

With a weave, the flow is guaranteed to start at the top and "fall" to the bottom. Flow errors are impossible in a basic weave structure, and the output text can be easily skim read. That means there's no need to actually test all the branches in game to be sure they work as intended.

Weaves also allow for easy redrafting of choice-points; in particular, it's easy to break a sentence up and insert additional choices for variety or pacing reasons, without having to re-engineer any flow.


## 2) Nested Flow

The weaves shown above are quite simple, "flat" structures. Whatever the player does, they take the same number of turns to get from top to bottom. However, sometimes certain choices warrant a bit more depth or complexity.

For that, we allow weaves to nest.

This section comes with a warning. Nested weaves are very powerful and very compact, but they can take a bit of getting used to!

### Options can be nested

Consider the following scene:

	- 	"Well, Poirot? Murder or suicide?"
	*	"Murder!"
	* 	"Suicide!"
	-	Ms. Christie lowered her manuscript a moment. The rest of the writing group sat, open-mouthed.

The first choice presented is "Murder!" or "Suicide!". If Poirot declares a suicide, there's no more to do, but in the case of murder, there's a follow-up question needed - who does he suspect?

We can add new options via a set of nested sub-choices. We tell the script that these new choices are "part of" another choice by using two asterisks, instead of just one.


	- 	"Well, Poirot? Murder or suicide?"
		*	"Murder!"
		 	"And who did it?"
			* * 	"Detective-Inspector Japp!"
			* * 	"Captain Hastings!"
			* * 	"Myself!"
		* 	"Suicide!"
		-	Mrs. Christie lowered her manuscript a moment. The rest of the writing group sat, open-mouthed.

(Note that it's good style to also indent the lines to show the nesting, but the compiler doesn't mind.)

And should we want to add new sub-options to the other route, we do that in similar fashion.

	- 	"Well, Poirot? Murder or suicide?"
		*	"Murder!"
		 	"And who did it?"
			* * 	"Detective-Inspector Japp!"
			* * 	"Captain Hastings!"
			* * 	"Myself!"
		* 	"Suicide!"
			"Really, Poirot? Are you quite sure?"
			* * 	"Quite sure."
			* *		"It is perfectly obvious."
		-	Mrs. Christie lowered her manuscript a moment. The rest of the writing group sat, open-mouthed.

Now, that initial choice of accusation will lead to specific follow-up questions - but either way, the flow will come back together at the gather point, for Mrs. Christie's cameo appearance.

But what if we want a more extended sub-scene?

### Gather points can be nested too

Sometimes, it's not a question of expanding the number of options, but having more than one additional beat of story. We can do this by nesting gather points as well as options.

	- 	"Well, Poirot? Murder or suicide?"
			*	"Murder!"
			 	"And who did it?"
				* * 	"Detective-Inspector Japp!"
				* * 	"Captain Hastings!"
				* * 	"Myself!"
				- - 	"You must be joking!"
				* * 	"Mon ami, I am deadly serious."
				* *		"If only..."
			* 	"Suicide!"
				"Really, Poirot? Are you quite sure?"
				* * 	"Quite sure."
				* *		"It is perfectly obvious."
			-	Mrs. Christie lowered her manuscript a moment. The rest of the writing group sat, open-mouthed.

If the player chooses the "murder" option, they'll have two choices in a row on their sub-branch - a whole flat weave, just for them.

#### Дополнительно: What gathers do

Gathers are hopefully intuitive, but their behaviour is a little harder to put into words: in general, after an option has been taken, the story finds the next gather down that isn't on a lower level, and diverts to it.

The basic idea is this: options separate the paths of the story, and gathers bring them back together. (Hence the name, "weave"!)


### You can nest as many levels are you like

Above, we used two levels of nesting; the main flow, and the sub-flow. But there's no limit to how many levels deep you can go.

	-	"Tell us a tale, Captain!"
		*	"Very well, you sea-dogs. Here's a tale..."
			* * 	"It was a dark and stormy night..."
					* * * 	"...and the crew were restless..."
							* * * *  "... and they said to their Captain..."
									* * * * *		"...Tell us a tale Captain!"
		*	"No, it's past your bed-time."
 	-	To a man, the crew began to yawn.

After a while, this sub-nesting gets hard to read and manipulate, so it's good style to divert away to a new stitch if a side-choice goes unwieldy.

But, in theory at least, you could write your entire story as a single weave.

### Example: a conversation with nested nodes

Here's a longer example:

	- I looked at Monsieur Fogg
	*	... and I could contain myself no longer.
		'What is the purpose of our journey, Monsieur?'
		'A wager,' he replied.
		* * 	'A wager!'[] I returned.
				He nodded.
				* * * 	'But surely that is foolishness!'
				* * *  'A most serious matter then!'
				- - - 	He nodded again.
				* * *	'But can we win?'
						'That is what we will endeavour to find out,' he answered.
				* * *	'A modest wager, I trust?'
						'Twenty thousand pounds,' he replied, quite flatly.
				* * * 	I asked nothing further of him then[.], and after a final, polite cough, he offered nothing more to me. <>
		* * 	'Ah[.'],' I replied, uncertain what I thought.
		- - 	After that, <>
	*	... but I said nothing[] and <>
	- we passed the day in silence.
	- -> END

with a couple of possible playthroughs. A short one:

	I looked at Monsieur Fogg

	1: ... and I could contain myself no longer.
	2: ... but I said nothing

	> 2
	... but I said nothing and we passed the day in silence.

and a longer one:

	I looked at Monsieur Fogg

	1: ... and I could contain myself no longer.
	2: ... but I said nothing

	> 1
	... and I could contain myself no longer.
	'What is the purpose of our journey, Monsieur?'
	'A wager,' he replied.

	1: 'A wager!'
	2: 'Ah.'

	> 1
	'A wager!' I returned.
	He nodded.

	1: 'But surely that is foolishness!'
	2: 'A most serious matter then!'

	> 2
	'A most serious matter then!'
	He nodded again.

	1: 'But can we win?'
	2: 'A modest wager, I trust?'
	3: I asked nothing further of him then.

	> 2
	'A modest wager, I trust?'
	'Twenty thousand pounds,' he replied, quite flatly.
	After that, we passed the day in silence.

Hopefully, this demonstrates the philosophy laid out above: that weaves offer a compact way to offer a lot of branching, a lot of choices, but with the guarantee of getting from beginning to end!


## 3) Tracking a Weave

Sometimes, the weave structure is sufficient. But when it's not, we need a bit more control.

### Weaves are largely unaddressed

By default, lines of content in a weave don't have an address or label, which means they can't be diverted to, and they can't be tested for. In the most basic weave structure, choices vary the path the player takes through the weave and what they see, but once the weave is finished those choices and that path are forgotten.

But should we want to remember what the player has seen, we can - we add in labels where they're needed using the `(label_name)` syntax.

### Gathers and options can be labelled

Gather points at any nested level can be labelled using brackets.

	-  (top)

Once labelled, gather points can be diverted to, or tested for in conditionals, just like knots and stitches. This means you can use previous decisions to alter later outcomes inside the weave, while still keeping all the advantages of a clear, reliable forward-flow.

Options can also be labelled, just like gather points, using brackets. Label brackets come before conditions in the line.

These addresses can be used in conditional tests, which can be useful for creating options unlocked by other options.

	=== meet_guard ===
	The guard frowns at you.

	* 	(greet) [Greet him]
		'Greetings.'
	*	(get_out) 'Get out of my way[.'],' you tell the guard.

	- 	'Hmm,' replies the guard.

	*	{greet} 	'Having a nice day?' // only if you greeted him

	* 	'Hmm?'[] you reply.

	*	{get_out} [Shove him aside] 	 // only if you threatened him
		You shove him sharply. He stares in reply, and draws his sword!
		-> fight_guard 			// this route diverts out of the weave

	-	'Mff,' the guard replies, and then offers you a paper bag. 'Toffee?'


### Scope

Inside the same block of weave, you can simply use the label name; from outside the block you need a path, either to a different stitch within the same knot:

	=== knot ===
	= stitch_one
		- (gatherpoint) Some content.
	= stitch_two
		*	{stitch_one.gatherpoint} Option

or pointing into another knot:

	=== knot_one ===
	-	(gather_one)
		* {knot_two.stitch_two.gather_two} Option

	=== knot_two ===
	= stitch_two
		- (gather_two)
			*	{knot_one.gather_one} Option


#### Дополнительно: all options can be labelled

In truth, all content in ink is a weave, even if there are no gathers in sight. That means you can label *any* option in the game with a bracket label, and then reference it using the addressing syntax. In particular, this means you can test *which* option a player took to reach a particular outcome.

	=== fight_guard ===
	...
	= throw_something
	*	(rock) [Throw rock at guard] -> throw
	* 	(sand) [Throw sand at guard] -> throw

	= throw
	You hurl {throw_something.rock:a rock|a handful of sand} at the guard.


#### Дополнительно: Loops in a weave

Labelling allows us to create loops inside weaves. Here's a standard pattern for asking questions of an NPC.

	- (opts)
		*	'Can I get a uniform from somewhere?'[] you ask the cheerful guard.
			'Sure. In the locker.' He grins. 'Don't think it'll fit you, though.'
		*	'Tell me about the security system.'
			'It's ancient,' the guard assures you. 'Old as coal.'
		*	'Are there dogs?'
			'Hundreds,' the guard answers, with a toothy grin. 'Hungry devils, too.'
		// We require the player to ask at least one question
		*	{loop} [Enough talking]
			-> done
	- (loop)
		// loop a few times before the guard gets bored
		{ -> opts | -> opts | }
		He scratches his head.
		'Well, can't stand around talking all day,' he declares.
	- (done)
		You thank the guard, and move away.





#### Дополнительно: diverting to options

Options can also be diverted to: but the divert goes to the output of having chosen that choice, *as though the choice had been chosen*. So the content printed will ignore square bracketed text, and if the option is once-only, it will be marked as used up.

	- (opts)
	*	[Pull a face]
		You pull a face, and the soldier comes at you! -> shove

	*	(shove) [Shove the guard aside] You shove the guard to one side, but he comes back swinging.

	*	{shove} [Grapple and fight] -> fight_the_guard

	- 	-> opts

produces:

	1: Pull a face
	2: Shove the guard aside

	> 1
	You pull a face, and the soldier comes at you! You shove the guard to one side, but he comes back swinging.

	1: Grapple and fight

	>

#### Дополнительно: Gathers directly after an option

The following is valid, and frequently useful.

	*	"Are you quite well, Monsieur?"[] I asked.
		- - (quitewell) "Quite well," he replied.
	*	"How did you do at the crossword, Monsieur?"[] I asked.
		-> quitewell
	*	I said nothing[] and neither did my Master.
	-	We feel into companionable silence once more.

Note the level 2 gather point directly below the first option: there's nothing to gather here, really, but it gives us a handy place to divert the second option to.






# Part 3: Variables and Logic

So far we've made conditional text, and conditional choices, using tests based on what content the player has seen so far.

**ink** also supports variables, both temporary and global, storing numerical and content data, or even story flow commands. It is fully-featured in terms of logic, and contains a few additional structures to help keep the often complex logic of a branching story better organised.


## 1) Global Variables

The most powerful kind of variable, and arguably the most useful for a story, is a variable to store some unique property about the state of the game - anything from the amount of money in the protagonist's pocket, to a value representing the protagonist's state of mind.

This kind of variable is called "global" because it can be accessed from anywhere in the story - both set, and read from. (Traditionally, programming tries to avoid this kind of thing, as it allows one part of a program to mess with another, unrelated part. But a story is a story, and stories are all about consequences: what happens in Vegas rarely stays there.)

### Defining Global Variables

Global variables can be defined anywhere, via a `VAR` statement. They should be given an initial value, which defines what type of variable they are - integer, floating point (decimal), content, or a story address.

	VAR knowledge_of_the_cure = false
	VAR players_name = "Emilia"
	VAR number_of_infected_people = 521
	VAR current_epilogue = -> they_all_die_of_the_plague

### Using Global Variables

We can test global variables to control options, and provide conditional text, in a similar way to what we have previously seen.

	=== the_train ===
		The train jolted and rattled. { mood > 0:I was feeling positive enough, however, and did not mind the odd bump|It was more than I could bear}.
		*	{ not knows_about_wager } 'But, Monsieur, why are we travelling?'[] I asked.
		* 	{ knows_about_wager} I contemplated our strange adventure[]. Would it be possible?

#### Дополнительно: storing diverts as variables

A "divert" statement is actually a type of value in itself, and can be stored, altered, and diverted to.

	VAR 	current_epilogue = -> everybody_dies

	=== continue_or_quit ===
	Give up now, or keep trying to save your Kingdom?
	*  [Keep trying!] 	-> more_hopeless_introspection
	*  [Give up] 		-> current_epilogue


#### Дополнительно: Global variables are externally visible

Global variables can be accessed, and altered, from the runtime as well from the story, so provide a good way to communicate between the wider game and the story.

The **ink** layer is often be a good place to store gameplay-variables; there's no save/load issues to consider, and the story itself can react to the current values.



### Printing variables

The value of a variable can be printed as content using an inline syntax similar to sequences, and conditional text:

	VAR friendly_name_of_player = "Jackie"
	VAR age = 23

	"My name is Jean Passepartout, but my friend's call me {friendly_name_of_player}. I'm {age} years old."

This can be useful in debugging. For more complex printing based on logic and variables, see the section on functions.

## 2) Logic

Obviously, our global variables are not intended to be constants, so we need a syntax for altering them.

Since by default, any text in an **ink** script is printed out directly to the screen, we use a markup symbol to indicate that a line of content is intended meant to be doing some numerical work, we use the `~` mark.

The following statements all assign values to variables:


	=== set_some_variables ===
		~ knows_about_wager = true
		~ x = (x * x) - (y * y) + c
		~ y = 2 * x * y

### Mathematics

**ink** supports the four basic mathematical operations (`+`, `-`, `*` and `/`), as well as `%` (or `mod`), which returns the remainder after integer division.

If more complex operations are required, one can write functions (for recursive formulas and the like), or call out to external, game-code functions (for anything more advanced).

#### Дополнительно: numerical types are implicit

Results of operations - in particular, for division - are typed based on the type of the input. So integer division returns integer, but floating point division returns floating point results.

	~ x = 2 / 3
	~ y = 7 / 3
	~ z = 1.2 / 0.5

assigns `x` to be 0, `y` to be 2 and `z` to be 2.4.

## 3) Conditional blocks (if/else)

We've seen conditionals used to control options and story content; **ink** also provides an equivalent of the normal if/else-if/else structure.

### A simple 'if'

The if syntax takes its cue from the other conditionals used so far, with the `{`...`}` syntax indicating that something is being tested.

	{ x > 0:
		~ y = x - 1
	}

Else conditions can be provided:

	{ x > 0:
		~ y = x - 1
	- else:
		~ y = x + 1
	}

### Extended if/else if/else blocks

The above syntax is actually a specific case of a more general structure, something like a "switch" statement of another language:

	{
		- x > 0:
			~ y = x - 1
		- else:
			~ y = x + 1
	}

And using this form we can include 'else-if' conditions:

	{
		- x == 0:
			~ y = 0
		- x > 0:
			~ y = x - 1
		- else:
			~ y = x + 1
	}

(Note, as with everything else, the white-space is purely for readability and has no syntactic meaning.)

#### Example: context-relevant content

Note these tests don't have to be variable-based and can use read-counts, just as other conditionals can, and the following construction is quite frequent, as a way of saying "do some content which is relevant to the current game state":

	=== dream ===
		{
			- visited_snakes && not dream_about_snakes:
				~ fear++
				-> dream_about_snakes

			- visited_poland && not dream_about_polish_beer:
				~ fear--
				-> dream_about_polish_beer

			- else:
				// breakfast-based dreams have no effect
				-> dream_about_marmalade
		}

The syntax has the advantage of being easy to extend, and prioritise.



### Conditional blocks are not limited to logic

Conditional blocks can be used to control story content as well as logic:

	I stared at Monsieur Fogg.
	{ know_about_wager:
		<> "But surely you are not serious?" I demanded.
	- else:
		<> "But there must be a reason for this trip," I observed.
	}
	He said nothing in reply, merely considering his newspaper with as much thoroughness as entomologist considering his latest pinned addition.

You can even put options inside conditional blocks:

	{ door_open:
		* 	I strode out of the compartment[] and I fancied I heard my master quietly tutting to himself. 			-> go_outside
	- else:
		*	I asked permission to leave[] and Monsieur Fogg looked surprised. 	-> open_door
		* 	I stood and went to open the door[]. Monsieur Fogg seemed untroubled by this small rebellion. -> open_door
	}

...but note that the lack of weave-syntax and nesting in the above example isn't accidental: to avoid confusing the various kinds of nesting at work, you aren't allowed to include gather points inside conditional blocks.

### Multiline list blocks

There's one other class of multiline block, which expands on the list functionality previous mentioned. The following are all valid and do what you might expect:

 	// Sequence: go through the alternatives, and stick on last
	{ stopping:
		-	I entered the casino.
		-  I entered the casino again.
		-  Once more, I went inside.
	}

	// Shuffle: show one at random
	At the table, I drew a card. <>
	{ shuffle:
		- 	Ace of Hearts.
		- 	King of Spades.
		- 	2 of Diamonds.
			'You lose!' crowed the croupier.
			-> leave_casino
	}

	// Cycle: show each in turn, and then cycle
	{ cycle:
		- I held my breath.
		- I waited impatiently.
		- I paused.
	}

	// Once: show each, once, in turn, until all have been shown
	{ once:
		- Would my luck hold?
		- Could I win the hand?
	}

## 4) Temporary Variables

### Temporary variables are for scratch calculations

Sometimes, a global variable is unwieldy. **ink** provides temporary variables for quick calculations of things.  

	=== near_north_pole ===
		~ temp number_of_warm_things = 0
		{ blanket:
			~ number_of_warm_things++
		}
		{ ear_muffs:
			~ number_of_warm_things++
		}
		{ gloves:
			~ number_of_warm_things++
		}
		{ number_of_warm_things > 2:
			Despite the snow, I felt incorrigibly snug.
		- else:
			That night I was colder than I have ever been.
		}

The value in a temporary variable is thrown away after the story leaves the knot in which it was defined.

TODO: check this is actually true

### Knots and stitches can take parameters

A particularly useful form of temporary variable is a parameter. Any knot or stitch can be given a value as a parameter.

	*	[Accuse Hasting]
			-> accuse("Hastings")
	*	[Accuse Mrs Black]
			-> accuse("Claudia")
	*	[Accuse myself]
			-> accuse("myself")

	=== accuse(who) ===
		"I accuse {who}!" Poirot declared.
		"Really?" Japp replied. "{who == "myself":You did it?|{who}?}"
		"And why not?" Poirot shot back. 	




#### Example: a recursive knot definition

Temporary variables are safe to use in recursion (unlike globals), so the following will work.

	-> add_one_to_one_hundred(0, 1)

	=== add_one_to_one_hundred(total, x) ===
		~ total = total + x
		{ x == 100:
			-> finished(total)
		- else:
			-> add_one_to_one_hundred(total, x + 1)
		}

	=== finished(total) ===
		"The result is {total}!" you announce.
		Gauss stares at you in horror.
		-> END


(In fact, this kind of definition is useful enough that **ink** provides a special kind of knot, called, imaginatively enough, a `function`, which comes with certain restrictions and can return a value. See the section below.)


#### Дополнительно: sending divert targets as parameters

Knot/stitch addresses are a type of value, indicated by a `->` character, and can be stored and passed around. The following is therefore legal, and often useful:

	=== sleeping_in_hut ===
		You lie down and close your eyes.
		-> generic_sleep (-> waking_in_the_hut)

	===	 generic_sleep (-> waking)
		You sleep perchance to dream etc. etc.
		-> waking

	=== waking_in_the_hut
		You get back to your feet, ready to continue your journey.

...but note the `->` in the `generic_sleep` definition: that's the one case in **ink** where a parameter needs to be typed: because it's too easy to otherwise accidentally do the following:

	=== sleeping_in_hut ===
		You lie down and close your eyes.
		-> generic_sleep (waking_in_the_hut)

... which sends the read count of `waking_in_the_hut` into the sleeping knot, and then attempts to divert to it.





## 5) Functions

The use of parameters on knots means they are almost functions in the usual sense, but they lack one key concept - that of the call stack, and the use of return values.

**ink** includes functions: they are knots, with the following limitations and features:

A function:
- cannot contain stitches
- cannot use diverts or offer choices
- can call other functions
- can include printed content
- can return a value of any type
- can recurse safely

(Some of these may seem quite limiting, but for more story-oriented call-stack-style features, see the section of Tunnels.)

Return values are provided via the `~ return` statement.

### Defining and calling functions

To define a function, simply declare a knot to be one:

	=== function say_yes_to_everything ===
		~ return true

	=== function lerp(a, b, k) ===
		~ return ((b - a) * k) + a

Functions are called by name, and with brackets, even if they have no parameters:

	~ x = lerp(2, 8, 0.3)

	*	{say_yes_to_everything()} 'Yes.'

As in any other language, a function, once done, returns the flow to wherever it was called from - and despite not being allowed to divert the flow, functions can still call other functions.

	=== function say_no_to_nothing ===
		~ return say_yes_to_everything()

### Functions don't have to return anything

A function does not need to have a return value, and can simply do something that is worth packaging up:

	=== function harm(x) ===
		{ stamina < x:
			~ stamina = 0
		- else:
			~ stamina = stamina - x
		}

...though remember a function cannot divert, so while the above prevents a negative Stamina value, it won't kill a player who hits zero.

### Functions can be called inline

Functions can be called on `~` content lines, but can also be called during a piece of content. In this context, the return value, if there is one, is printed (as well as anything else the function wants to print.) If there is no return value, nothing is printed.

Content is, by default, 'glued in', so the following:

	Monsieur Fogg was looking {describe_health(health)}.

	=== function describe_health(x) ===
	{
	- x == 100:
		~ return "spritely"
	- x > 75:
		~ return "chipper"
	- x > 45:
		~ return "somewhat flagging"
	- else:
		~ return "despondent"
	}

produces:

	Monsieur Fogg was looking despondent.

#### Examples

For instance, you might include:

	=== function max(a,b) ===
		{ a < b:
			~ return b
		- else:
			~ return a
		}

	=== function exp(x, e) ===
		// returns x to the power e where e is an integer
		{ e <= 0:
			~ return 1
		- else:
			~ return x * exp(x, e - 1)
		}

Then:

	The maximum of 2^5 and 3^3 is {max(exp(2,5), exp(3,3))}.

produces:

	The maximum of 2^5 and 3^3 is 32.


#### Example: turning numbers into words

The following example is long, but appears in pretty much every inkle game to date. (Recall that a hyphenated line inside multiline curly braces indicates either "a condition to test" or, if the curly brace began with a variable, "a value to compare against".)

    === function print_num(x) ===
    {
        - x >= 1000:
            {print_num(x / 1000)} thousand { x mod 1000 > 0:{print_num(x mod 1000)}}
        - x >= 100:
            {print_num(x / 100)} hundred { x mod 100 > 0:and {print_num(x mod 100)}}
        - x == 0:
            zero
        - else:
            { x >= 20:
                { x / 10:
                    - 2: twenty
                    - 3: thirty
                    - 4: forty
                    - 5: fifty
                    - 6: sixty
                    - 7: seventy
                    - 8: eighty
                    - 9: ninety
                }
                { x mod 10 > 0:<>-<>}
            }
            { x < 10 || x > 20:
                { x mod 10:
                    - 1: one
                    - 2: two
                    - 3: three
                    - 4: four        
                    - 5: five
                    - 6: six
                    - 7: seven
                    - 8: eight
                    - 9: nine
                }
            - else:     
                { x:
                    - 10: ten
                    - 11: eleven       
                    - 12: twelve
                    - 13: thirteen
                    - 14: fourteen
                    - 15: fifteen
                    - 16: sixteen      
                    - 17: seventeen
                    - 18: eighteen
                    - 19: nineteen
                }
            }
    }

which enables us to write things like:

	~ price = 15

	I pulled out {print_num(price)} coins from my pocket and slowly counted them.
	"Oh, never mind," the trader replied. "I'll take half." And she took {print_num(price / 2)}, and pushed the rest back over to me.



### Parameters can be passed by reference

Function parameters can also be passed 'by reference', meaning that the function can actually alter the the variable being passed in, instead of creating a temporary variable with that value.

For instance, most **inkle** stories include the following:

	=== function alter(ref x, k) ===
		~ x = x + k

Lines such as:

	~ gold = gold + 7
	~ health = health - 4

then become:

	~ alter(gold, 7)
	~ alter(health, -4)

which are slightly easier to read, and (more usefully) can be done inline for maximum compactness.

	*	I ate a biscuit[] and felt refreshed. {alter(health, 2)}
	* 	I gave a biscuit to Monsieur Fogg[] and he wolfed it down most undecorously. {alter(foggs_health, 1)}
	-	<> Then we continued on our way.

Wrapping up simple operations in function can also provide a simple place to put debugging information, if required.




##  6) Constants


### Global Constants

Interactive stories often rely on state machines, tracking what stage some higher level process has reached. There are lots of ways to do this, but the most conveninent is to use constants.

Sometimes, it's convenient to define constants to be strings, so you can print them out, for gameplay or debugging purposes.

	CONST HASTINGS = "Hastings"
	CONST POIROT = "Poirot"
	CONST JAPP = "Japp"

	VAR current_chief_suspect = HASTINGS

	=== review_evidence ===
		{ found_japps_bloodied_glove:
			~ current_chief_suspect = POIROT
		}
		Current Suspect: {current_chief_suspect}

Sometimes giving them values is useful:

	CONST PI = 3.14
	CONST VALUE_OF_TEN_POUND_NOTE = 10

And sometimes the numbers are useful in other ways:

	CONST LOBBY = 1
	CONST STAIRCASE = 2
	CONST HALLWAY = 3

	CONST HELD_BY_AGENT = -1

	VAR secret_agent_location = LOBBY
	VAR suitcase_location = HALLWAY

	=== report_progress ===
	{  secret_agent_location = suitcase_location:
		The secret agent grabs the suitcase!
		~ suitcase_location = HELD_BY_AGENT  

	-  secret_agent_location < suitcase_location:
		The secret agent moves forward.
		~ secret_agent_location++
	}

Constants are simply a way to allow you to give story states easy-to-understand names.  

## 7) Дополнительно: Game-side logic

There are two core ways to provide game hooks in the **ink** engine. External function declarations in ink allow you to directly call C# functions in the game, and variable observers are callbacks that are fired in the game when ink variables are modified. Both of these are described in [Running your ink](https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md).

# Part 4: Advanced Flow Control


## 1) Tunnels

The default structure for **ink** stories is a "flat" tree of choices, branching and joining back together, perhaps looping, but with the story always being "at a certain place".

But this flat structure makes certain things difficult: for example, imagine a game in which the following interaction can happen:

	=== crossing_the_date_line ===
	*	"Monsieur!"[] I declared with sudden horror. "I have just realised. We have crossed the international date line!"
	-	Monsieur Fogg barely lifted an eyebrow. "I have adjusted for it."
	*	I mopped the sweat from my brow[]. A relief!
	* 	I nodded, becalmed[]. Of course he had!
	*  I cursed, under my breath[]. Once again, I had been belittled!

...but it can happen at several different places in the story. We don't want to have to write copies of the content for each different place, but when the content is finished it needs to know where to return to. We can do this using parameters:

	=== crossing_the_date_line(-> return_to) ===
	...
	-	-> return_to

	...

	=== outside_honolulu ===
	We arrived at the large island of Honolulu.
	- (postscript)
		{crossing_the_date_line(-> done)}
	- (done)
		-> END

	...

	=== outside_pitcairn_island ===
	The boat sailed along the water towards the tiny island.
	- (postscript)
		{crossing_the_date_line(-> done)}
	- (done)
		-> END

Both of these locations now call and execute the same segment of storyflow, but once finished they return to where they need to go next.

But what if the section of story being called is more complex - what if it spreads across several knots? Using the above, we'd have to keep passing the 'return-to' parameter from knot to knot, to ensure we always knew where to return.

So instead, **ink** integrates this into the language with a new kind of divert, that functions rather like a subroutine, and is called a 'tunnel'.

### Tunnels run sub-stories

The tunnel syntax looks like a divert, with another divert on the end:

	-> crossing_the_date_line ->

This means "do the crossing_the_date_line story, then continue from here".

Inside the tunnel itself, the syntax is simplified from the parameterised example: all we do is end the tunnel using the `->->` statement which means, essentially, "go on".

	=== crossing_the_date_line ===
	// this is a tunnel!
	...
	- 	->->

Note that tunnel knots aren't declared as such, so the compiler won't check that tunnels really do end in `->->` statements, except at run-time. So you will need to write carefully to ensure that all the flows into a tunnel really do come out again.

Tunnels can also be chained together, or finish on a normal divert:

	...
	// this runs the tunnel, then diverts to 'done'
	-> crossing_the_date_line -> done
	...

	...
	//this runs one tunnel, then another, then diverts to 'done'
	-> crossing_the_date_line -> check_foggs_health -> done
	...

Tunnels can be nested, so the following is valid:

	=== plains ===
	= night_time
		The dark grass is soft under your feet.
		+	[Sleep]
			-> sleep_here -> wake_here -> day_time
	= day_time
		It is time to move on.

	=== wake_here ===
		You wake as the sun rises.
		+	[Eat something]
			-> eat_something ->
		+	[Make a move]
		-	->->

	=== sleep_here ===
		You lie down and try to close your eyes.
		-> monster_attacks ->
		Then it is time to sleep.
		-> dream ->
		->->

... and so on.



#### Дополнительно: Tunnels use a call-stack

Tunnels are on a call-stack, so can safely recurse.


## 2) Threads

So far, everything in ink has been entirely linear, despite all the branching and diverting. But it's actually possible for a writer to 'fork' a story into different sub-sections, to cover more possible player actions.

We call this 'threading', though it's not really threading in the sense that computer scientists mean it: it's more like stitching in new content from various places.

Note that this is definitely an advanced feature: the engineering stories becomes somewhat more complex once threads are involved!

### Threads join multiple sections together

Threads allow you to compose sections of content from multiple sources in one go. For example:

    == thread_example ==
    I had a headache; threading is hard to get your head around.
    <- conversation
    <- walking


    == conversation ==
    It was a tense moment for Monty and me.
     * "What did you have for lunch today?"[] I asked.
        "Spam and eggs," he replied.
     * "Nice weather, we're having,"[] I said.
        "I've seen better," he replied.
     - -> house

    == walking ==
    We continued to walk down the dusty road.
     * [Continue walking]
        -> house

    == house ==
    Before long, we arrived at his house.
    -> END

It allows multiple sections of story to combined together into a single section:

    I had a headache; threading is hard to get your head around.
    It was a tense moment for Monty and me.
    We continued to walk down the dusty road.
    1: "What did you have for lunch today?"
    2: "Nice weather, we're having,"
    3: Continue walking

On encountering a thread statement such as `<- conversation`, the compiler will fork the story flow. The first fork considered will run the content at `conversation`, collecting up any options it finds. Once it has run out of flow here it'll then run the other fork.

All the content is collected and shown to the player. But when a choice is chosen, the engine will move to that fork of the story and collapse and discard the others.

Note that global variables are *not* forked, including the read counts of knots and stitches.

### Uses of threads

In a normal story, threads might never be needed.

But for games with lots of independent moving parts, threads quickly become essential. Imagine a game in which characters move independently around a map: the main story hub for a room might look like the following:

	CONST HALLWAY = 1
	CONST OFFICE = 2

	VAR player_location = HALLWAY
	VAR generals_location = HALLWAY
	VAR doctors_location = OFFICE

	== run_player_location
		{
			- player_location == HALLWAY: -> hallway
		}

	== hallway ==
		<- characters_present
		*	[Drawers]	-> examine_drawers
		* 	[Wardrobe] -> examine_wardrobe
		*  [Go to Office] 	-> go_office
		-	-> run_player_location
	= examine_drawers
		// etc...

	// Here's the thread, which mixes in dialogue for characters you share the room with at the moment.

	== characters_present(room)
		{ generals_location == player_location:
			<- general_conversation
		}
		{ doctors_location == room:
			<- doctor_conversation
		}

	== general_conversation
		*	[Ask the General about the bloodied knife]
			"It's a bad business, I can tell you."
		-	-> run_player_location

	== doctor_conversation
		*	[Ask the Doctor about the bloodied knife]
			"There's nothing strange about blood, is there?"
		-	-> run_player_location



Note in particular, that we need an explicit way to return the player who has gone down a side-thread to return to the main flow. In most cases, threads will either need a parameter telling them where to return to, or they'll need to end the current story section.


### When does a side-thread end?

Side-threads end when they run out of flow to process: and note, they collect up options to display later (unlike tunnels, which collect options, display them and follow them until they hit an explicit return, possibly several moves later).

Sometimes a thread has no content to offer - perhaps there is no conversation to have with a character after all, or perhaps we have simply not written it yet. In that case, we must mark the end of the thread explicitly.

If we didn't, the end of content might be a story-bug or a hanging story thread, and we want the compiler to tell us about those.

### Using `-> DONE`

So cases where we want to mark the end of a thread, we use `-> DONE`: meaning "the flow intentionally ends here".

Note that we don't need a `-> DONE` if the flow ends with options that fail their conditions. The engine treats this as a valid, intentional, end of flow state.

**You do not need a `-> DONE` in a thread after an option has been chosen**. Once an option is chosen, a thread is no longer a thread - it is simply the normal story flow once more.

Using `-> END` in this case will not end the thread, but the whole story flow. (And this is the real reason for having two different ways to end flow.)


#### Example: adding the same choice to several places

Threads can be used to add the same choice into lots of different places. When using them this way, it's normal to pass a divert as a parameter, to tell the story where to go after the choice is done.

	=== outside_the_house
	The front step. The house smells. Of murder. And lavender.
	- (top)
		<- review_case_notes(-> top)
		*	[Go through the front door]
			I stepped inside the house.
			-> the_hallway
		* 	[Sniff the air]
			I hate lavender. It makes me think of soap, and soap makes me think about my marriage.
			-> top

	=== the_hallway
	The hallway. Front door open to the street. Little bureau.
	- (top)
		<- review_case_notes(-> top)
		*	[Go through the front door]
			I stepped out into the cool sunshine.
			-> outside_the_house
		* 	[Open the bureau]
			Keys. More keys. Even more keys. How many locks do these people need?
			-> top

	=== review_case_notes(-> go_back_to)
	+	{not done || TURNS_SINCE(-> done) > 10}
		[Review my case notes]
		// the conditional ensures you don't get the option to check repeatedly
	 	{I|Once again, I} flicked through the notes I'd made so far. Still not obvious suspects.
	- 	(done) -> go_back_to

Note this is different than a tunnel, which runs the same block of content but doesn't give a player a choice. So a layout like:

	<- childhood_memories(-> next)
	*	[Look out of the window]
	 	I daydreamed as we rolled along...
	 - (next) Then the whistle blew...

might do exactly the same thing as:

	*	[Remember my childhood]
		-> think_back ->
	*	[Look out of the window]
		I daydreamed as we rolled along...
	- 	(next) Then the whistle blew... 	

but as soon as the option being threaded in includes multiple choices, or conditional logic on choices (or any text content, of course!), the thread version becomes more practical.


#### Example: organisation of wide choice points

A game which uses ink as a script rather than a literal output might often generate very large numbers of parallel choices, intended to be filtered by the player via some other in-game interaction - such as walking around an environment. Threads can be useful in these cases simply to divide up choices.

```
=== the_kitchen
- (top)
	<- drawers(-> top)
	<- cupboards(-> top)
	<- room_exits
= drawers (-> goback)
	// choices about the drawers...
	...
= cupboards(-> goback)
	// choices about cupboards
	...
= room_exits
	// exits; doesn't need a "return point" as if you leave, you go elsewhere
	...
```

# Part 5: Advanced State Tracking

Games with lots of interaction can get very complex, very quickly and the writer's job is often as much about maintaining continuity as it is about content.

This becomes particularly important if the game text is intended to model anything - whether it's a game of cards, the player's knowledge of the gameworld so far, or the state of the various light-switches in a house.

**ink** does not provide a full world-modelling system in the manner of a classic parser IF authoring language - there are no "objects", no concepts of "containment" or being "open" or "locked". However, it does provide a simple yet powerful system for tracking state-changes in a very flexible way, to enable writers to approximate world models where necessary.

#### Note: New feature alert!

This feature is very new to the language. That means we haven't begun to discover all the ways it might be used - but we're pretty sure it's going to be useful! So if you think of a clever usage we'd love to know!


## 1) Basic Lists

The basic unit of state-tracking is a list of states, defined using the `LIST` keyword. Note that a list is really nothing like a C# list (which is an array).

For instance, we might have:

	LIST kettleState = cold, boiling, recently_boiled

This line defines two things: firstly three new values - `cold`, `boiling` and `recently_boiled` - and secondly, a variable, called `kettleState`, to hold these states.

We can tell the list what value to take:

	~ kettleState = cold

We can change the value:

	*	[Turn on kettle]
		The kettle begins to bubble and boil.
		~ kettleState = boiling

We can query the value:

	*	[Touch the kettle]
		{ kettleState == cold:
			The kettle is cool to the touch.
		- else:
		 	The outside of the kettle is very warm!
		}

For convenience, we can give a list a value when it's defined using a bracket:

	LIST kettleState = cold, (boiling), recently_boiled
	// at the start of the game, this kettle is switched on. Edgy, huh?

...and if the notation for that looks a bit redundant, there's a reason for that coming up in a few subsections time.



## 2) Reusing Lists

The above example is fine for the kettle, but what if we have a pot on the stove as well? We can then define a list of states, but put them into variables - and as many variables as we want.

	LIST daysOfTheWeek = Monday, Tuesday, Wednesday, Thursday, Friday
	VAR today = Monday
	VAR tomorrow = Tuesday

### States can be used repeatedly

This allows us to use the same state machine in multiple places.

	LIST heatedWaterStates = cold, boiling, recently_boiled
	VAR kettleState = cold
	VAR potState = cold

	*	{kettleState == cold} [Turn on kettle]
		The kettle begins to boil and bubble.
		~ kettleState = boiling
	*	{potState == cold} [Light stove]
	 	The water in the pot begins to boil and bubble.
	 	~ potState = boiling

But what if we add a microwave as well? We might want start generalising our functionality a bit:

	LIST heatedWaterStates = cold, boiling, recently_boiled
	VAR kettleState = cold
	VAR potState = cold
	VAR microwaveState = cold

	=== function boilSomething(ref thingToBoil, nameOfThing)
		The {nameOfThing} begins to heat up.
		~ thingToBoil = boiling

	=== do_cooking
	*	{kettleState == cold} [Turn on kettle]
		{boilSomething(kettleState, "kettle")}
	*	{potState == cold} [Light stove]
		{boilSomething(potState, "pot")}		*	{microwaveState == cold} [Turn on microwave]
		{boilSomething(microwaveState, "microwave")}

or even...

	LIST heatedWaterStates = cold, boiling, recently_boiled
	VAR kettleState = cold
	VAR potState = cold
	VAR microwaveState = cold

	=== cook_with(nameOfThing, ref thingToBoil)
	+ 	{thingToBoil == cold} [Turn on {nameOfThing}]
	  	The {nameOfThing} begins to heat up.
		~ thingToBoil = boiling
		-> do_cooking.done

	=== do_cooking
	<- cook_with("kettle", kettleState)
	<- cook_with("pot", potState)
	<- cook_with("microwave", microwaveState)
	- (done)

Note that the "heatedWaterStates" list is still available as well, and can still be tested, and take a value.

#### List values can share names

Reusing lists brings with it ambiguity. If we have:

	LIST colours = red, green, blue, purple
	LIST moods = mad, happy, blue

	VAR status = blue

... how can the compiler know which blue you meant?

We resolve these using a `.` syntax similar to that used for knots and stitches.

	VAR status = colours.blue

...and the compiler will issue an error until you specify.

Note the "family name" of the state, and the variable containing a state, are totally separate. So

	{ statesOfGrace == statesOfGrace.fallen:
		// is the current state "fallen"
	}

... is correct.


#### Дополнительно: a LIST is actually a variable

One surprising feature is the statement

	LIST statesOfGrace = ambiguous, saintly, fallen

actually does two things simultaneously: it creates three values, `ambiguous`, `saintly` and `fallen`, and gives them the name-parent `statesOfGrace` if needed; and it creates a variable called `statesOfGrace`.

And that variable can be used like a normal variable. So the following is valid, if horribly confusing and a bad idea:

	LIST statesOfGrace = ambiguous, saintly, fallen

	~ statesOfGrace = 3.1415 // set the variable to a number not a list value

...and it wouldn't preclude the following from being fine:

	~ temp anotherStateOfGrace = statesOfGrace.saintly




## 3) List Values

When a list is defined, the values are listed in an order, and that order is considered to be significant. In fact, we can treat these values as if they *were* numbers. (That is to say, they are enums.)

	LIST volumeLevel = off, quiet, medium, loud, deafening
	VAR lecturersVolume = quiet
	VAR murmurersVolume = quiet

	{ lecturersVolume < deafening:
		~ lecturersVolume++

		{ lecturersVolume > murmurersVolume:
			~ murmurersVolume++
			The murmuring gets louder.
		}
	}

The values themselves can be printed using the usual `{...}` syntax, but this will print their name.

	The lecturer's voice becomes {lecturersVolume}.

### Converting values to numbers

The numerical value, if needed, can be got explicitly using the LIST_VALUE function. Note the first value in a list has the value 1, and not the value 0.

	The lecturer has {LIST_VALUE(deafening) - LIST_VALUE(lecturersVolume)} notches still available to him.

### Converting numbers to values

You can go the other way by using the list's name as a function:

	LIST Numbers = one, two, three
	VAR score = one
	~ score = Numbers(2) // score will be "two"

### Дополнительно: defining your own numerical values

By default, the values in a list start at 1 and go up by one each time, but you can specify your own values if you need to.

	LIST primeNumbers = two = 2, three = 3, five = 5

If you specify a value, but not the next value, ink will assume an increment of 1. So the following is the same:

	LIST primeNumbers = two = 2, three, five = 5


## 4) Multivalued Lists

The following examples have all included one deliberate untruth, which we'll now remove. Lists - and variables containing list values - do not have to contain only one value.

### Lists are boolean sets  

A list variable is not a variable containing a number. Rather, a list is like the in/out nameboard in an accommodation block. It contains a list of names, each of which has a room-number associated with it, and a slider to say "in" or "out".

Maybe no one is in:

	LIST DoctorsInSurgery = Adams, Bernard, Cartwright, Denver, Eamonn

Maybe everyone is:

	LIST DoctorsInSurgery = (Adams), (Bernard), (Cartwright), (Denver), (Eamonn)

Or maybe some are and some aren't:

	LIST DoctorsInSurgery = (Adams), Bernard, (Cartwright), Denver, Eamonn

Names in brackets are included in the initial state of the list.

Note that if you're defining your own values, you can place the brackets around the whole term or just the name:

	LIST primeNumbers = (two = 2), (three) = 3, (five = 5)

#### Assiging multiple values

We can assign all the values of the list at once as follows:

	~ DoctorsInSurgery = (Adams, Bernard)
	~ DoctorsInSurgery = (Adams, Bernard, Eamonn)

We can assign the empty list to clear a list out:

	~ DoctorsInSurgery = ()


#### Adding and removing entries

List entries can be added and removed, singly or collectively.

	~ DoctorsInSurgery = DoctorsInSurgery + Adams 	~ DoctorsInSurgery += Adams  // this is the same as the above
	~ DoctorsInSurgery -= Eamonn
	~ DoctorsInSurgery += (Eamonn, Denver)
	~ DoctorsInSurgery -= (Adams, Eamonn, Denver)

Trying to add an entry that's already in the list does nothing. Trying to remove an entry that's not there also does nothing. Neither produces an error, and a list can never contain duplicate entries.


### Basic Queries

We have a few basic ways of getting information about what's in a list:

	LIST DoctorsInSurgery = (Adams), Bernard, (Cartwright), Denver, Eamonn

	{LIST_COUNT(DoctorsInSurgery)} 	//  "2"
	{LIST_MIN(DoctorsInSurgery)} 		//  "Adams"
	{LIST_MAX(DoctorsInSurgery)} 		//  "Cartwright"

#### Testing for emptiness

Like most values in ink, a list can be tested "as it is", and will return true, unless it's empty.

	{ DoctorsInSurgery: The surgery is open today. | Everyone has gone home. }

#### Testing for exact equality

Testing multi-valued lists is slightly more complex than single-valued ones. Equality (`==`) now means 'set equality' - that is, all entries are identical.

So one might say:

	{ DoctorsInSurgery == (Adams, Bernard):
		Dr Adams and Dr Bernard are having a loud argument in one corner.
	}

If Dr Eamonn is in as well, the two won't argue, as the lists being compared won't be equal - DoctorsInSurgery will have an Eamonn that the list (Adams, Bernard) doesn't have.

Not equals works as expected:

	{ DoctorsInSurgery != (Adams, Bernard):
		At least Adams and Bernard aren't arguing.
	}

#### Testing for containment

What if we just want to simply ask if Adams and Bernard are present? For that we use a new operator, `has`, otherwise known as `?`.

	{ DoctorsInSurgery ? (Adams, Bernard):
		Dr Adams and Dr Bernard are having a hushed argument in one corner.
	}

And `?` can apply to single values too:

	{ DoctorsInSurgery has Eamonn:
		Dr Eamonn is polishing his glasses.
	}

We can also negate it, with `hasnt` or `!?` (not `?`). Note this starts to get a little complicated as

	DoctorsInSurgery !? (Adams, Bernard)

does not mean neither Adams nor Bernard is present, only that they are not *both* present (and arguing).


#### Example: basic knowledge tracking

The simplest use of a multi-valued list is for tracking "game flags" tidily.

	LIST Facts = (Fogg_is_fairly_odd), 	first_name_phileas, (Fogg_is_English)

	{Facts ? Fogg_is_fairly_odd:I smiled politely.|I frowned. Was he a lunatic?}
	'{Facts ? first_name_phileas:Phileas|Monsieur}, really!' I cried.

In particular, it allows us to test for multiple game flags in a single line.

	{ Facts ? (Fogg_is_English, Fogg_is_fairly_odd):  
		<> 'I know Englishmen are strange, but this is *incredible*!'
	}


#### Example: a doctor's surgery

We're overdue a fuller example, so here's one.

	LIST DoctorsInSurgery = (Adams), Bernard, Cartwright, (Denver), Eamonn

	-> waiting_room

	=== function whos_in_today()
		In the surgery today are {DoctorsInSurgery}.

	=== function doctorEnters(who)
		{ DoctorsInSurgery !? who:
			~ DoctorsInSurgery += who
			Dr {who} arrives in a fluster.
		}

	=== function doctorLeaves(who)
		{ DoctorsInSurgery ? who:
			~ DoctorsInSurgery -= who
			Dr {who} leaves for lunch.
		}

	=== waiting_room
		{whos_in_today()}
		*	[Time passes...]
			{doctorLeaves(Adams)} {doctorEnters(Cartwright)} {doctorEnters(Eamonn)}
			{whos_in_today()}

This produces:

	In the surgery today are Adams, Denver.

	> Time passes...

	Dr Adams leaves for lunch. Dr Cartwright arrives in a fluster. Dr Eamonn arrives in a fluster.

	In the surgery today are Cartwright, Denver, Eamonn.

#### Дополнительно: nicer list printing

The basic list print is not especially attractive for use in-game. The following is better:

	=== function listWithCommas(list, if_empty)
	    {LIST_COUNT(list):
	    - 2:
	        	{LIST_MIN(list)} and {listWithCommas(list - LIST_MIN(list), if_empty)}
	    - 1:
	        	{list}
	    - 0:
				{if_empty}	        
	    - else:
	      		{LIST_MIN(list)}, {listWithCommas(list - LIST_MIN(list), if_empty)}
	    }

	LIST favouriteDinosaurs = (stegosaurs), brachiosaur, (anklyosaurus), (pleiosaur)

	My favourite dinosaurs are {listWithCommas(favouriteDinosaurs, "all extinct")}.

It's probably also useful to have an is/are function to hand:

	=== function isAre(list)
		{LIST_COUNT(list) == 1:is|are}

	My favourite dinosaurs {isAre(favouriteDinosaurs)} {listWithCommas(favouriteDinosaurs, "all extinct")}.

And to be pendantic:

	My favourite dinosaur{LIST_COUNT(favouriteDinosaurs) != 1:s} {isAre(favouriteDinosaurs)} {listWithCommas(favouriteDinosaurs, "all extinct")}.


#### Lists don't need to have multiple entries

Lists don't *have* to contain multiple values. If you want to use a list as a state-machine, the examples above will all work - set values using `=`, `++` and `--`; test them using `==`, `<`, `<=`, `>` and `>=`. These will all work as expected.

### The "full" list

Note that `LIST_COUNT`, `LIST_MIN` and `LIST_MAX` are refering to who's in/out of the list, not the full set of *possible* doctors. We can access that using

	LIST_ALL(element of list)

or

	LIST_ALL(list containing elements of a list)

	{LIST_ALL(DoctorsInSurgery)} // Adams, Bernard, Cartwright, Denver, Eamonn
	{LIST_COUNT(LIST_ALL(DoctorsInSurgery))} // "5"
	{LIST_MIN(LIST_ALL(Eamonn))} 				// "Adams"

Note that printing a list using `{...}` produces a bare-bones representation of the list; the values as words, delimited by commas.

#### Дополнительно: "refreshing" a list's type

If you really need to, you can make an empty list that knows what type of list it is.

	LIST ValueList = first_value, second_value, third_value
	VAR myList = ()

	~ myList = ValueList()

You'll then be able to do:

	{ LIST_ALL(myList) }

#### Дополнительно: a portion of the "full" list

You can also retrieve just a "slice" of the full list, using the `LIST_RANGE` function.

	LIST_RANGE(list_name, min_value, max_value)

### Example: Tower of Hanoi

To demonstrate a few of these ideas, here's a functional Tower of Hanoi example, written so no one else has to write it.


	LIST Discs = one, two, three, four, five, six, seven
	VAR post1 = ()
	VAR post2 = ()
	VAR post3 = ()

	~ post1 = LIST_ALL(Discs)

	-> gameloop

	=== function can_move(from_list, to_list) ===
	    {
	    -   LIST_COUNT(from_list) == 0:
	        // no discs to move
	        ~ return false
	    -   LIST_COUNT(to_list) > 0 && LIST_MIN(from_list) > LIST_MIN(to_list):
	        // the moving disc is bigger than the smallest of the discs on the new tower
	        ~ return false
	    -   else:  
	    	 // nothing stands in your way!
	        ~ return true

	    }

	=== function move_ring( ref from, ref to ) ===
	    ~ temp whichRingToMove = LIST_MIN(from)
	    ~ from -= whichRingToMove
	    ~ to += whichRingToMove

	== function getListForTower(towerNum)
	    { towerNum:
	        - 1:    ~ return post1
	        - 2:    ~ return post2
	        - 3:    ~ return post3
	    }

	=== function name(postNum)
	    the {postToPlace(postNum)} temple

	=== function Name(postNum)
	    The {postToPlace(postNum)} temple

	=== function postToPlace(postNum)
	    { postNum:
	        - 1: first
	        - 2: second
	        - 3: third
	    }

	=== function describe_pillar(listNum) ==
	    ~ temp list = getListForTower(listNum)
	    {
	    - LIST_COUNT(list) == 0:
	        {Name(listNum)} is empty.
	    - LIST_COUNT(list) == 1:
	        The {list} ring lies on {name(listNum)}.
	    - else:
	        On {name(listNum)}, are the discs numbered {list}.
	    }


	=== gameloop
	    Staring down from the heavens you see your followers finishing construction of the last of the great temples, ready to begin the work.
	- (top)
	    +  (describe) {true || TURNS_SINCE(-> describe) >= 2 || !describe} [ Regard the temples]
	        You regard each of the temples in turn. On each is stacked the rings of stone. {describe_pillar(1)} {describe_pillar(2)} {describe_pillar(3)}
	    <- move_post(1, 2, post1, post2)   
	    <- move_post(2, 1, post2, post1)  
	    <- move_post(1, 3, post1, post3)  
	    <- move_post(3, 1, post3, post1)  
	    <- move_post(3, 2, post3, post2)  
	    <- move_post(2, 3, post2, post3)  
	    -> DONE

	= move_post(from_post_num, to_post_num, ref from_post_list, ref to_post_list)
	    +   { can_move(from_post_list, to_post_list) }
	        [ Move a ring from {name(from_post_num)} to {name(to_post_num)} ]
	        { move_ring(from_post_list, to_post_list) }
	        { stopping:
	        -   The priests far below construct a great harness, and after many years of work, the great stone ring is lifted up into the air, and swung over to the next of the temples.
	            The ropes are slashed, and in the blink of an eye it falls once more.
	        -   Your next decree is met with a great feast and many sacrifices. After the funeary smoke has cleared, work to shift the great stone ring begins in earnest. A generation grows and falls, and the ring falls into its ordained place.
	        -   {cycle:
	            - Years pass as the ring is slowly moved.
	            - The priests below fight a war over what colour robes to wear, but while they fall and die, the work is still completed.
	            }
	        }
	    -> top



## 5) Advanced List Operations

The above section covers basic comparisons. There are a few more powerful features as well, but - as anyone familiar with mathematical   sets will know - things begin to get a bit fiddly. So this section comes with an 'advanced' warning.

A lot of the features in this section won't be necessary for most games.

### Comparing lists

We can compare lists less than exactly using `>`, `<`, `>=` and `<=`. Be warned! The definitions we use are not exactly standard fare. They are based on comparing the numerical value of the elements in the lists being tested.

#### "Distinctly bigger than"

`LIST_A > LIST_B` means "the smallest value in A is bigger than the largest values in B": in other words, if put on a number line, the entirety of A is to the right of the entirety of B. `<` does the same in reverse.

#### "Definitely never smaller than"

`LIST_A >= LIST_B` means - take a deep breath now - "the smallest value in A is at least the smallest value in B, and the largest value in A is at least the largest value in B". That is, if drawn on a number line, the entirety of A is either above B or overlaps with it, but B does not extend higher than A.

Note that `LIST_A > LIST_B` implies `LIST_A != LIST_B`, and `LIST_A >= LIST_B` allows `LIST_A == LIST_B` but precludes `LIST_A < LIST_B`, as you might hope.

#### Health warning!

`LIST_A >= LIST_B` is *not* the same as `LIST_A > LIST_B or LIST_A == LIST_B`.

The moral is, don't use these unless you have a clear picture in your mind.

### Inverting lists

A list can be "inverted", which is the equivalent of going through the accommodation in/out name-board and flipping every switch to the opposite of what it was before.

	LIST GuardsOnDuty = (Smith), (Jones), Carter, Braithwaite

	=== function changingOfTheGuard
		~ GuardsOnDuty = LIST_INVERT(GuardsOnDuty)


Note that `LIST_INVERT` on an empty list will return a null value, if the game doesn't have enough context to know what invert. If you need to handle that case, it's safest to do it by hand:

	=== function changingOfTheGuard
		{!GuardsOnDuty: // "is GuardsOnDuty empty right now?"
			~ GuardsOnDuty = LIST_ALL(Smith)
		- else:
			~ GuardsOnDuty = LIST_INVERT(GuardsOnDuty)
		}

#### Footnote

The syntax for inversion was originally `~ list` but we changed it because otherwise the line

	~ list = ~ list

was not only functional, but actually caused list to invert itself, which seemed excessively perverse.

### Intersecting lists

The `has` or `?` operator is, somewhat more formally, the "are you a subset of me" operator, ⊇, which includes the sets being equal, but which doesn't include if the larger set doesn't entirely contain the smaller set.

To test for "some overlap" between lists, we use the overlap operator, `^`, to get the *intersection*.

	LIST CoreValues = strength, courage, compassion, greed, nepotism, self_belief, delusions_of_godhood
	VAR desiredValues = (strength, courage, compassion, self_belief )
	VAR actualValues =  ( greed, nepotism, self_belief, delusions_of_godhood )

	{desiredValues ^ actualValues} // prints "self_belief"  

The result is a new list, so you can test it:

	{desiredValues ^ actualValues: The new president has at least one desirable quality.}

	{LIST_COUNT(desiredValues ^ actualValues) == 1: Correction, the new president has only one desirable quality. {desiredValues ^ actualValues == self_belief: It's the scary one.}}




## 6) Multi-list Lists


So far, all of our examples have included one large simplification, again - that the values in a list variable have to all be from the same list family. But they don't.

This allows us to use lists - which have so far played the role of state-machines and flag-trackers - to also act as general properties, which is useful for world modelling.

This is our inception moment. The results are powerful, but also more like "real code" than anything that's come before.

### Lists to track objects

For instance, we might define:

	LIST Characters = Alfred, Batman, Robin
	LIST Props = champagne_glass, newspaper

	VAR BallroomContents = (Alfred, Batman, newspaper)
	VAR HallwayContents = (Robin, champagne_glass)

We could then describe the contents of any room by testing its state:

	=== function describe_room(roomState)
		{ roomState ? Alfred: Alfred is here, standing quietly in a corner. } { roomState ? Batman: Batman's presence dominates all. } { roomState ? Robin: Robin is all but forgotten. }
		<> { roomState ? champagne_glass: A champagne glass lies discarded on the floor. } { roomState ? newspaper: On one table, a headline blares out WHO IS THE BATMAN? AND *WHO* IS HIS BARELY-REMEMBERED ASSISTANT? }

So then:

	{ describe_room(BallroomContents) }

produces:

	Alfred is here, standing quietly in a corner. Batman's presence dominates all.

	On one table, a headline blares out WHO IS THE BATMAN? AND *WHO* IS HIS BARELY-REMEMBERED ASSISTANT?

While:

	{ describe_room(HallwayContents) }

gives:

	Robin is all but forgotten.

	A champagne glass lies discarded on the floor.

And we could have options based on combinations of things:

	*	{ currentRoomState ? (Batman, Alfred) } [Talk to Alfred and Batman]
		'Say, do you two know each other?'

### Lists to track multiple states

We can model devices with multiple states. Back to the kettle again...

	LIST OnOff = on, off
	LIST HotCold = cold, warm, hot

	VAR kettleState = off, cold

	=== function turnOnKettle() ===
	{ kettleState ? hot:
		You turn on the kettle, but it immediately flips off again.
	- else:
		The water in the kettle begins to heat up.
		~ kettleState -= off
		~ kettleState += on
		// note we avoid "=" as it'll remove all existing states
	}

	=== function can_make_tea() ===
		~ return kettleState ? (hot, off)

These mixed states can make changing state a bit trickier, as the off/on above demonstrates, so the following helper function can be useful.

 	=== function changeStateTo(ref stateVariable, stateToReach)
 		// remove all states of this type
 		~ stateVariable -= LIST_ALL(stateToReach)
 		// put back the state we want
 		~ stateVariable += stateToReach

 which enables code like:

 	~ changeState(kettleState, on)
 	~ changeState(kettleState, warm)


#### How does this affect queries?

The queries given above mostly generalise nicely to multi-valued lists

    LIST Letters = a,b,c
    LIST Numbers = one, two, three

    VAR mixedList = (a, three, c)

	{LIST_ALL(mixedList)}   // a, one, b, two, c, three
    {LIST_COUNT(mixedList)} // 3
    {LIST_MIN(mixedList)}   // a
    {LIST_MAX(mixedList)}   // three or c, albeit unpredictably

    {mixedList ? (a,b) }        // false
    {mixedList ^ LIST_ALL(a)}   // a, c

    { mixedList >= (one, a) }   // true
    { mixedList < (three) }     // false

	{ LIST_INVERT(mixedList) }            // one, b, two


## 7) Long example: crime scene

Finally, here's a long example, demonstrating a lot of ideas from this section in action. You might want to try playing it before reading through to better understand the various moving parts.

	-> murder_scene

	//
	// 	System: items can have various states
	//	Some are general, some specific to particular items
	//

	LIST OffOn = off, on
	LIST SeenUnseen = unseen, seen

	LIST GlassState = (none), steamed, steam_gone
	LIST BedState = (made_up), covers_shifted, covers_off, stain_visible

	//
	// System: inventory
	//

	LIST Inventory = (none), cane, knife

	=== function get(x)
	    ~ Inventory += x

	//
	// System: positioning things
	// Items can be put in and on places
	//

	LIST Supporters = on_desk, on_floor, on_bed, under_bed, held, with_joe

	=== function move_to_supporter(ref item_state, new_supporter) ===
	    ~ item_state -= LIST_ALL(Supporters)
	    ~ item_state += new_supporter

	//
	// System: Incremental knowledge.
	// Each list is a chains of facts. Each fact supercedes the fact before it.
	//


	LIST BedKnowledge = (none), neatly_made, crumpled_duvet, hastily_remade, body_on_bed, murdered_in_bed, murdered_while_asleep
	LIST KnifeKnowledge = (none), prints_on_knife, joe_seen_prints_on_knife,joe_wants_better_prints, joe_got_better_prints
	LIST WindowKnowledge = (none), steam_on_glass, fingerprints_on_glass, fingerprints_on_glass_match_knife

	VAR knowledgeState = ()

	=== function learn(x) ===
		// learn this fact
	    ~ knowledgeState += x

	=== function learnt(x) ===
		// have you learnt this fact, or indeed a stronger one
	    ~ return highest_state_for_set_of_state(x) >= x

	=== function between(x, y) ===
		// are you between two ideas? Not necessarily in the same knowledge tree.
	    ~ return learnt(x) && not learnt(y)

	=== function think(x) ===
		// is this your current "strongest" idea in this knowledge set?
	    ~ return highest_state_for_set_of_state(x) == x

	=== function highest_state_for_set_of_state(x) ===
	    ~ return LIST_MAX(knowledgeState ^ LIST_ALL(x))

	=== function did_learn(x) ===
		//	did you learn this particular fact?
	    ~ return knowledgeState ? x

	//
	// Set up the scene
	//

	VAR bedroomLightState = (off, on_desk)
	VAR knifeState = (under_bed)

	//
	// Content
	//

	=== murder_scene ===
	    The bedroom. This is where it happened. Now to look for clues.
	- (top)
	    { bedroomLightState ? seen:     <- seen_light  }
	    <- compare_prints(-> top)

	    *   (dobed) [The bed...]
	        The bed was low to the ground, but not so low something might not roll underneath. It was still neatly made.
	        ~ learn(neatly_made)
	        - - (bedhub)
	        * *     [Lift the bedcover]
	                I lifted back the bedcover. The duvet underneath was crumpled.
	                ~ learn(crumpled_duvet)
	                ~ BedState = covers_shifted
	        * *     (uncover) {learnt(crumpled_duvet)}
	                [Remove the cover]
	                Careful not to disturb anything beneath, I removed the cover entirely. The duvet below was rumpled.
	                Not the work of the maid, who was conscientious to a point. Clearly this had been thrown on in a hurry.
	                ~ learn(hastily_remade)
	                ~ BedState = covers_off
	        * *     (duvet) {BedState == covers_off} [ Pull back the duvet ]
	                I pulled back the duvet. Beneath it was a sheet, sticky with blood.
	                ~ BedState = stain_visible
	                ~ learn(body_on_bed)
	                Either the body had been moved here before being dragged to the floor - or this is was where the murder had taken place.
	        * *     {!(BedState ? made_up)} [ Remake the bed ]
	                Carefully, I pulled the bedsheets back into place, trying to make it seem undisturbed.
	                ~ BedState = made_up
	        * *     [Test the bed]
	                I pushed the bed with spread fingers. It creaked a little, but not so much as to be obnoxious.
	        * *     (darkunder) [Look under the bed]
	                Lying down, I peered under the bed, but could make nothing out.

	        * *     {TURNS_SINCE(-> dobed) > 1} [Something else?]
	                I took a step back from the bed and looked around.
	                -> top
	        - -     -> bedhub

	    *   {darkunder && bedroomLightState ? on_floor && bedroomLightState ? on}
	        [ Look under the bed ]
	        I peered under the bed. Something glinted back at me.
	        - - (reaching)
	        * *     [ Reach for it ]
	                I fished with one arm under the bed, but whatever it was, it had been kicked far enough back that I couldn't get my fingers on it.
	                -> reaching
	        * *     {Inventory ? cane} [Knock it with the cane]
	                -> knock_with_cane

	        * *     {reaching > 1 } [ Stand up ]
	                I stood up once more, and brushed my coat down.
	                -> top

	    *   (knock_with_cane) {reaching && TURNS_SINCE(-> reaching) >= 4 &&  Inventory ? cane } [Use the cane to reach under the bed ]
	        Positioning the cane above the carpet, I gave the glinting thing a sharp tap. It slid out from the under the foot of the bed.
	        ~ move_to_supporter( knifeState, on_floor )
	        * *     (standup) [Stand up]
	                Satisfied, I stood up, and saw I had knocked free a bloodied knife.
	                -> top
	        * *     [Look under the bed once more]
	                Moving the cane aside, I looked under the bed once more, but there was nothing more there.
	                -> standup        

	    *   {knifeState ? on_floor} [Pick up the knife]
	        Careful not to touch the handle, I lifted the blade from the carpet.
	        ~ get(knife)

	    *   {Inventory ? knife} [Look at the knife]
	        The blood was dry enough. Dry enough to show up partial prints on the hilt!
	        ~ learn(prints_on_knife)

	    *   [   The desk... ]
	        I turned my attention to the desk. A lamp sat in one corner, a neat, empty in-tray in the other. There was nothing else out.
	        Leaning against the desk was a wooden cane.
	        ~ bedroomLightState += seen
	        - - (deskstate)
	        * *     (pickup_cane) {Inventory !? cane}    [Pick up the cane ]
	                ~ get(cane)
	                I picked up the wooden cane. It was heavy, and unmarked.

	        * *    { bedroomLightState !? on } [Turn on the lamp]
	                -> operate_lamp ->
	        * *     [Look at the in-tray ]
	                I regarded the in-tray, but there was nothing to be seen. Either the victim's papers were taken, or his line of work had seriously dried up. Or the in-tray was all for show.
	        + +     (open)  {open < 3} [Open a drawer]
	                I tried {a drawer at random|another drawer|a third drawer}. {Locked|Also locked|Unsurprisingly, locked as well}.

	        * *     {deskstate >= 2} [Something else?]
	                I took a step away from the desk once more.
	                -> top
	        - -     -> deskstate

	    *     {(Inventory ? cane) && TURNS_SINCE(-> deskstate) <= 2} [Swoosh the cane]    
	        I was still holding the cane: I gave it an experimental swoosh. It was heavy indeed, though not heavy enough to be used as a bludgeon.
	        But it might have been useful in self-defence. Why hadn't the victim reached for it? Knocked it over?

	    *   [The window...]
	        I went over to the window and peered out. A dismal view of the little brook that ran down beside the house.
	        - - (window_opts)
	        <- compare_prints(-> window_opts)
	        * *     (downy) [Look down at the brook]
	                { GlassState ? steamed:
	                    Through the steamed glass I couldn't see the brook. -> see_prints_on_glass -> window_opts
	                }
	                I watched the little stream rush past for a while. The house probably had damp but otherwise, it told me nothing.
	        * *     (greasy) [Look at the glass]
	                { GlassState ? steamed: -> downy }
	                The glass in the window was greasy. No one had cleaned it in a while, inside or out.
	        * *     { GlassState ? steamed && not see_prints_on_glass && downy && greasy }
	                [ Look at the steam ]
	                A cold day outside. Natural my breath should steam. -> see_prints_on_glass ->
	        + +     {GlassState ? steam_gone} [ Breathe on the glass ]
	                I breathed gently on the glass once more. {learnt(fingerprints_on_glass): The fingerprints reappeared. }
	                ~ GlassState = steamed

	        + +     [Something else?]
	                { window_opts < 2 || learnt(fingerprints_on_glass) || GlassState ? steamed:
	                    I looked away from the dreary glass.
	                    {GlassState ? steamed:
	                        ~ GlassState = steam_gone
	                        <> The steam from my breath faded.
	                    }
	                    -> top
	                }
	                I leant back from the glass. My breath had steamed up the pane a little.
	               ~ GlassState = steamed
	        - -     -> window_opts



	    *   {top >= 5} [Leave the room]
	        I'd seen enough. I {bedroomLightState ? on:switched off the lamp, then} turned and left the room.
	        -> joe_in_hall
	    -   -> top

	= see_prints_on_glass
	    ~ learn(fingerprints_on_glass)
	    {But I could see a few fingerprints, as though someone had leant their palm against it.|The fingerprints were quite clear and well-formed.} They faded as I watched.   
	    ~ GlassState = steam_gone
	    ->->

	= compare_prints (-> backto)
	    *   {learnt(fingerprints_on_glass) && learnt(prints_on_knife) && !learnt(fingerprints_on_glass_match_knife)} [Compare the prints on the knife and the window ]    
	        Holding the bloodied knife near the window, I breathed to bring out the prints once more, and compared them as best I could.
	        Hardly scientific, but they seemed very similar - very similiar indeed.
	        ~ learn(fingerprints_on_glass_match_knife)
	        -> backto

	= operate_lamp
	    I flicked the light switch.
	    { bedroomLightState ? on:
	        <> The bulb fell dark.
	        ~ bedroomLightState += off
	        ~ bedroomLightState -= on
	    - else:
	        { bedroomLightState ? on_floor: <> A little light spilled under the bed.} { bedroomLightState ? on_desk : <> The light gleamed on the polished tabletop. }
	        ~ bedroomLightState -= off
	        ~ bedroomLightState += on
	    }
	    ->->

	= seen_light  
	    *   {!(bedroomLightState ? on)} [ Turn on lamp ]
	        -> operate_lamp ->

	    *   { !(bedroomLightState ? on_bed)  && BedState ? stain_visible }
	        [ Move the light to the bed ]
	        ~ move_to_supporter(bedroomLightState, on_bed)
	        I moved the light over to the bloodstain and peered closely at it. It had soaked deeply into the fibres of the cotton sheet.
	        There was no doubt about it. This was where the blow had been struck.
	        ~ learn(murdered_in_bed)

	    *   { !(bedroomLightState ? on_desk) } {TURNS_SINCE(-> floorit) >= 2 }
	        [ Move the light back to the desk ]
	        ~ move_to_supporter(bedroomLightState, on_desk)
	        I moved the light back to the desk, setting it down where it had originally been.
	    *   (floorit) { !(bedroomLightState ? on_floor) && darkunder }
	        [Move the light to the floor ]
	        ~ move_to_supporter(bedroomLightState, on_floor)
	        I picked the light up and set it down on the floor.
	    -   -> top

	=== joe_in_hall
	    My police contact, Joe, was waiting in the hall. 'So?' he demanded. 'Did you find anything interesting?'
	- (found)
	    *   {found == 1} 'Nothing.'
	        He shrugged. 'Shame.'
	        -> done
	    *   { Inventory ? knife } 'I found the murder weapon.'
	        'Good going!' Joe replied with a grin. 'We thought the murderer had gotten rid of it. I'll bag that for you now.'
	        ~ move_to_supporter(knifeState, with_joe)
	    *   {learnt(prints_on_knife)} { knifeState ? with_joe }
	        'There are prints on the blade[.'],' I told him.
	        He regarded them carefully.
	        'Hrm. Not very complete. It'll be hard to get a match from these.'
	        ~ learn(joe_seen_prints_on_knife)
	    *   { learnt(fingerprints_on_glass_match_knife) && learnt(joe_seen_prints_on_knife) }
	        'They match a set of prints on the window, too.'
	        'Anyone could have touched the window,' Joe replied thoughtfully. 'But if they're more complete, they should help us get a decent match!'
	        ~ learn(joe_wants_better_prints)
	    *   { between(body_on_bed, murdered_in_bed)}
	        'The body was moved to the bed at some point[.'],' I told him. 'And then moved back to the floor.'
	        'Why?'
	        * *     'I don't know.'
	                Joe nods. 'All right.'
	        * *     'Perhaps to get something from the floor?'
	                'You wouldn't move a whole body for that.'
	        * *     'Perhaps he was killed in bed.'
	                'It's just speculation at this point,' Joe remarks.
	    *   { learnt(murdered_in_bed) }
	        'The victim was murdered in bed, and then the body was moved to the floor.'
	        'Why?'
	        * *     'I don't know.'
	                Joe nods. 'All right, then.'
	        * *     'Perhaps the murderer wanted to mislead us.'
	                'How so?'
	            * * *   'They wanted us to think the victim was awake[.'], I replied thoughtfully. 'That they were meeting their attacker, rather than being stabbed in their sleep.'
	            * * *   'They wanted us to think there was some kind of struggle[.'],' I replied. 'That the victim wasn't simply stabbed in their sleep.'
	            - - -   'But if they were killed in bed, that's most likely what happened. Stabbed, while sleeping.'
	                    ~ learn(murdered_while_asleep)
	        * *     'Perhaps the murderer hoped to clean up the scene.'
	                'But they were disturbed? It's possible.'


	    *   { found > 1} 'That's it.'
	        'All right. It's a start,' Joe replied.
	        -> done
	    -   -> found
	-   (done)
	    {
	    - between(joe_wants_better_prints, joe_got_better_prints):
	        ~ learn(joe_got_better_prints)
	        <>  He moved for the door.  'I'll get those prints from the window now.'
	    - learnt(joe_seen_prints_on_knife):
	        <> 'I'll run those prints as best I can.'
	    - else:
	        <> 'Not much to go on.'
	    }
	    -> END

## 8) Summary

To summarise a difficult section, **ink**'s list construction provides:

### Flags
* 	Each list entry is an event
* 	Use `+=` to mark an event as having occurred
*  	Test using `?` and `!?`

Example:

	LIST GameEvents = foundSword, openedCasket, metGorgon
	{ GameEvents ? openedCasket }
	{ GameEvents ? (foundSword, metGorgon) }
	~ GameEvents += metGorgon

### State machines
* 	Each list entry is a state
*  Use `=` to set the state; `++` and `--` to step forward or backward
*  Test using `==`, `>` etc

Example:

	LIST PancakeState = ingredients_gathered, batter_mix, pan_hot, pancakes_tossed, ready_to_eat
	{ PancakeState == batter_mix }
	{ PancakeState < ready_to_eat }
	~ PancakeState++

### Properties
*	Each list is a different property, with values for the states that property can take (on or off, lit or unlit, etc)
* 	Change state by removing the old state, then adding in the new
*  Test using `?` and `!?`

Example:

	LIST OnOffState = on, off
	LIST ChargeState = uncharged, charging, charged

	VAR PhoneState = (off, uncharged)

	*	{PhoneState !? uncharged } [Plug in phone]
		~ PhoneState -= LIST_ALL(ChargeState)
		~ PhoneState += charging
		You plug the phone into charge.
	*	{ PhoneState ? (on, charged) } [ Call my mother ]




# Part 6: International character support in identifiers

By default, ink has no limitations on the use of non-ASCII characters inside the story content. However, a limitation currently exsits
on the characters that can be used for names of constants, variables, stictches, diverts and other named flow elements (a.k.a. *identifiers*).

Sometimes it is inconvenient for a writer using a non-ASCII language to write a story because they have to constantly switch to naming identifiers in ASCII and then switching back to whatever language they are using for the story. In addition, naming identifiers in the author's own language could improve the overal readibility of the raw story format.

In an effort to assist in the above scenario, ink *automatically* supports a list of pre-defined non-ASCII character ranges that can be used as identifiers. In general, those ranges have been selected to include the alpha-numeric subset of the official unicode character range, which would suffice for naming identifiers. The below section gives more detailed information on the non-ASCII characters that ink automatically supports.

### Supported Identifier Characters

The support for the additional character ranges in ink is currently limited to a predefined set of character ranges.

Below is a listing of the currently supported identifier ranges.

 - **Arabic**

   Enables characters for languages of the Arabic family and is a subset of the official *Arabic* unicode range `\u0600`-`\u06FF`.


 - **Armenian**

   Enables characters for the Armenian language and is a subset of the official *Armenian* unicode range `\u0530`-`\u058F`.


 - **Cyrillic**

   Enables characters for languages using the Cyrillic alphabet and is a subset of the official *Cyrillic* unicode range `\u0400`-`\u04FF`.


 - **Greek**  

   Enables characters for languages using the Greek alphabet and is a subset of the official *Greek and Coptic* unicode range `\u0370`-`\u03FF`.


 - **Hebrew**  

   Enables characters in Hebrew using the Hebrew alphabet and is a subset of the official *Hebrew* unicode range `\u0590`-`\u05FF`.


 - **Latin Extended A**  

   Enables an extended character range subset of the Latin alphabet - completely represented by the official *Latin Extended-A* unicode range `\u0100`-`\u017F`.


 - **Latin Extended B**

   Enables an extended character range subset of the Latin alphabet - completely represented by the official *Latin Extended-B* unicode range `\u0180`-`\u024F`.  


**NOTE!** ink files should be saved in UTF-8 format, which ensures that the above character ranges are supported.

If a particular character range that you would like to use within identifiers isn't supported, feel free to open an [issue](https://github.com/inkle/ink/issues/new) or [pull request](https://github.com/inkle/ink/pulls) on the main ink repo.
