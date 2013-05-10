Funktionalität: Versandkosten
	Damit Lilly & Hoonie, bei den Versandkosten nicht draufzahlen
	Und der Kunde auch nicht mehr als nötig zahlt
	Soll der Shop ie Versandkosten für eine Bestellung so gut wie möglich berechnen.

Szenariogrundriss: Ein Produkt soll nach Österreich verschickt werden
	Angenommen Produkte der Kategorie <Kategorie> werden verschickt
	Und der Zielland-Code ist <Land-Code>
	Dann sollen die Versandkosten <Kosten>€ betragen

	Beispiele: 
	| Kategorie         | Land-Code | Kosten |
	| klein             | at        | 3      |
	| klein,groß        | at        | 5      |
	| klein,groß,mittel | de        | 12     |
