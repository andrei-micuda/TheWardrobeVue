const haveCommonElements = (arr1, arr2) =>  arr1.some(element => {
  return arr2.includes(element);
});

module.exports = { haveCommonElements }