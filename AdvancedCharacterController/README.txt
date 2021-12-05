Come usare l'Advanced Movement

Il prefab Player contiene lo script 'PlayerInputs' che utilizza il vecchio input system di unity...
Questo va sostituito da uno script di input personale (nel caso usassi Rewired o simili)

AdvancedMovement:

Move(Vector2) - prende come argomento il vector2 dell'input di movimento premuto normalizzato;

Rotate(Vector2) - prende come argomento il vector2 dell'input del mouse NON normalizzato (inversione disponibile tra le variabili di movimento);

Running() - Input della corsa;

Crouch() - Stesso meccanismo del Run;

Jump() - Input del salto.