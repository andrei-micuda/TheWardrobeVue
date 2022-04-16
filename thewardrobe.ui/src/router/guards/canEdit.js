import store from "../../store";

function canEdit(itemId, next) {
  console.log("In canEdit guard.")
  var userItems = store.state.userItems;
  if (!userItems) {
    next("/sell");
    return;
  }

  console.log(userItems)
  console.log(itemId)
  
  if (userItems.some(i => i === itemId))
  {
    next();
  } else {
    console.log("Redirecting to /sell")
    next("/sell");
    }
}

export default canEdit;