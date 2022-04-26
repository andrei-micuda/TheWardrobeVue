const haveCommonElements = (arr1, arr2) =>  arr1.some(element => {
  return arr2.includes(element);
});

function groupBy(list, keyGetter) {
  if (!list)
    return;
  const map = new Map();
  list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
  });
  return map;
}

module.exports = { haveCommonElements, groupBy }