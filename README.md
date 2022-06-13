# AppleFrenzy

This game is an iteration on the [Apple Picker Prototype](https://github.com/Shaman37/Prototype-Apple_Picker), made in order to explore Unity. 

## Controls
The play can drag around a group of baskets and can click the mouse buttons to swap basket positions:

- **Left Mouse Click:** Swaps the baskets in a **clockwise** manner;
- **Right Mouse Click:** Swaps the baskets in a **counter-clockwise** manner;

## Objective
A player must catch the apples with the basket whose color matches that of the apple. Spoiled apples and Sticks can be caught by any basket. Therefore, we have:

- A green apple can only be caught by the green basket;
- A red apple can only be caught by the red basket;
- A golden apple can only be caught by the golden basket;
- A spoiled apple or stick can be caught by any basket but the player loses a life; 

## Scoring
As for scoring, the game has a combo bar, which progresses as the player picks good apples (green, red and golden). It works like this:

- Picking up a spoiled apple or a stick, resets the combo progress back to 0;
- Letting a good apple fall to the ground, resets the combo progress back to 0;
- To get a 2x multipler, a player need to catch 10 good apples in a row;
- To get a 3x multipler, a player need to catch 20 good apples in a row;
- To get a 4x multipler, a player need to catch 30 good apples in a row;

## Extra
A player is also rewarded if she's on a streak, where picking 100 apples good apples, gives you an extra life. But, ihe player drops 3 good apples in a row, she loses a life.

A gust of wind will also occasionally appear, making sticks fall from above, and add a bit of drag to the falling apples.

## Preview

https://user-images.githubusercontent.com/17680666/173307198-e0f16e37-527f-4e3e-aa82-5c9ccb65e5d1.mp4
