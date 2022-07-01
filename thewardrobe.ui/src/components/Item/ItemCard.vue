<template>
  <li>
    <router-link :to="itemLink">
      <div
        class="card w-full rounded bg-gray-800 transition ease-in-out relative"
        :class="{ 'opacity-70': !item.isAvailable }"
      >
        <p
          v-if="!item.isAvailable"
          class="
            absolute
            flex
            justify-center
            items-center
            px-2
            py-1
            rounded
            top-5
            left-5
            bg-yellow-400
            text-black
            z-50
          "
        >
          SOLD
        </p>
        <a-carousel arrows>
          <template #prevArrow>
            <div class="custom-slick-arrow z-10" style="left: 10px">
              <Icon
                icon="bi:arrow-left-circle-fill"
                width="20"
                height="20"
                class="text-gray-600"
              />
            </div>
          </template>
          <template #nextArrow>
            <div class="custom-slick-arrow" style="right: 10px">
              <Icon
                icon="bi:arrow-right-circle-fill"
                width="20"
                height="20"
                class="text-gray-600"
              />
            </div>
          </template>
          <div v-for="url in item.images" :key="url">
            <img :src="url" alt="" class="mx-auto w-100" />
          </div>
        </a-carousel>

        <a-row class="p-4 flex items-center">
          <a-col :span="16" class="text-left">
            <p class="price text-gray-100 text-lg">{{ item.price }} RON</p>
            <p class="text-gray-200">Size: {{ item.size }}</p>
            <p class="text-gray-200">Brand: {{ item.brand }}</p>
          </a-col>
          <a-col :span="8" class="flex justify-end items-center mr-2">
            <button @click.stop.prevent="toggleFavorite" class="z-100">
              <Icon
                :icon="item.isFavorite ? 'ci:heart-fill' : 'ci:heart-outline'"
                :width="32"
                :class="item.isFavorite ? 'text-red-400' : 'text-gray-100'"
              />
            </button>
          </a-col>
        </a-row>
      </div>
    </router-link>
  </li>
</template>

<script>
import { Icon } from "@iconify/vue2";

import api from "../../api";
import store from "../../store";
import router from "../../router";

export default {
  props: {
    editable: {
      type: Boolean,
    },
    item: {
      type: Object,
    },
  },
  computed: {
    itemLink() {
      if (this.editable)
        return { name: "editItem", params: { itemId: this.item.id } };

      return { name: "viewItem", params: { itemId: this.item.id } };
    },
  },
  methods: {
    toggleFavorite() {
      if (!store.state.id) {
        store.commit(
          "setWarningMsg",
          "The action you are trying to perform requires you to sign in first."
        );
        router.push("/signIn");
        return;
      }

      if (!this.item.isFavorite) {
        api
          .post(`/public/api/${store.state.id}/favorites`, {
            itemId: this.item.id,
          })
          .then(() => {
            this.item.isFavorite = true;
          });
      } else {
        api
          .delete(`/public/api/${store.state.id}/favorites`, {
            data: { itemId: this.item.id },
          })
          .then(() => {
            this.item.isFavorite = false;
          });
      }
    },
  },
  components: {
    Icon,
    // VButton
  },
};
</script>

<style lang="postcss" scoped>
.card {
  height: 350px;

  &:hover {
    -webkit-transform: scale(1.05);
    -moz-transform: scale(1.05);
    -o-transform: scale(1.05);
    transform: scale(1.05);
  }
}

.price {
  font-family: "Libre Franklin", sans-serif;
  font-weight: 500;
}

svg {
  position: relative;
}

img {
  height: 250px;
  object-fit: cover !important;
}
</style>
