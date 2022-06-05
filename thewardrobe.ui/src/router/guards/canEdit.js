import store from "../../store";
// TODO add guard for 403 error code
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