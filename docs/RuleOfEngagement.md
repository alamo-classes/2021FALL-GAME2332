# Rules of Engagement

## Player Input
When getting input from the player controls, Use the Input Manager instead of using the KeyCodes

```csharp
  //Should
  if ( Input.GetButtonDown( "NameOfButton" )
      //Player Action
  //Don't
  if ( Input.GetKeyDown( "KeyCode" )
      //Player Action
```
