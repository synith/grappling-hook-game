
##### August 9th, 2022
<br>




## Devlog v0.4
# Untitled [Grappling Hook Game](https://synith.itch.io/grappling-hook-game)





> <br> 
>  Delayed by a week as my attempt to refactor the codebase unleashed many bugs that I did not discover until I went to publish the build. <br>
> <br> 





## **New Features:**

* **Animations:**   
`Strafe Left/Right` <br>
`Walking Backwards` <br>
`Jump (Needs work)` <br>

* **Environment:** <br>
`Background Terrain` <br>
`Bushes` <br>
`Branches and Stumps` <br>
`Mushrooms and Flowers` <br>
`Grass and Rocks` <br>

* **Options:** <br> 
`Invert Camera Y` <br>
<br>




## **Bug Fixes:**

* **Game Over bug:**   
`Upon restarting the game your next collectable would set UI to 13/12, then the game over screen would not appear and input would be controlling the new character but also trying to control the old character` <br>
<br> 





## **Refactor:**


* **Animations:**   
`Less Singletons` <br>
`More Events` <br>
`Simple State Class` <br>
