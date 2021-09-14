<template>
  <div>
    <Icon
      :icon="this.icon"
      :width="size"
      :color="this.color"
      @click="toggleFavorite()"
    />
  </div>
</template>

<script>
import { Icon } from "@iconify/vue";

export default {
  name: "FavoriteButton",
  props: {
    size: String,
  },
  data() {
    return {
      color: "gray",
      icon: "ci:heart-outline",
    };
  },
  methods: {
    toggleFavorite() {
      this.color = this.color === "red" ? "gray" : "red";
      this.icon =
        this.icon === "ci:heart-outline" ? "ci:heart-fill" : "ci:heart-outline";
    },
  },
  components: { Icon },
};
</script>

<style scoped>
div {
  cursor: pointer;
  display: inline;
}
</style>