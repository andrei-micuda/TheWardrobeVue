import store from "../../store";

function canEdit(itemId, next) {
  var userItems = store.state.userItems;
  if (!userItems) {
    next("/sell");
    return;
  }

  if (userItems.some((i) => i === itemId)) {
    next();
  } else {
    next("/sell");
  }
}

export default canEdit;
