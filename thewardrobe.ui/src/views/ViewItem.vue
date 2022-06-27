<template>
  <div>
    <VPageHeader @back="$router.back()" title="View Item" />
    <a-row v-if="item" class="w-11/12 md:w-8/12 mx-auto py-2 md:px-10 md:py-8">
      <a-col :span="24" :md="{ span: 12 }">
        <a-carousel arrows class="bg-gray-800 mb-4 md:mb-0">
          <template #prevArrow>
            <div class="custom-slick-arrow" style="left: 10px; z-index: 10">
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
      </a-col>

      <a-col :span="24" :md="{ span: 12 }" class="md:pl-6 text-lg">
        <p class="text-2xl font-bold">{{ item.productName }}</p>
        <p class="text-lg">Brand: {{ item.brand }}</p>
        <p class="text-lg">Category: {{ item.category }}</p>
        <p class="text-lg">Gender: {{ item.gender }}</p>
        <p class="text-lg">Size: {{ item.size }}</p>
        <div class="flex items-center space-x-2">
          <p>Sold by: {{ item.seller }}</p>
          <div
            class="flex items-center space-x-1 bg-gray-800 px-3 py-1 rounded"
          >
            <p id="SellerRating" class="text-sm text-center">
              {{ item.sellerRating ? item.sellerRating.toFixed(2) : "N/A" }}
            </p>
            <Icon icon="ant-design:star-filled" width="16" />
          </div>
        </div>
        <p class="text-2xl font-bold">{{ item.price }} RON</p>

        <a-button
          :id="isInCart ? 'RemoveFromCartBtn' : 'AddToCartBtn'"
          :type="isInCart ? 'danger' : 'primary'"
          @click="toggleCart"
          class="px-5 py-3 uppercase my-4 font-bold hover:text-gray-600"
          large
        >
          {{ isInCart ? "Remove from cart" : "Add to cart" }}
          <i class="anticon"
            ><Icon icon="eva:shopping-cart-outline" :width="20"
          /></i>
        </a-button>
        <br />
        <a-button
          :id="item.isFavorite ? 'RemoveFromFavBtn' : 'SaveToFavBtn'"
          type="default"
          @click="toggleFavorite"
          class="px-5 py-3 uppercase font-bold"
          large
        >
          {{ item.isFavorite ? "Remove from favorites" : "Save to favorites" }}
          <i class="anticon"
            ><Icon
              :icon="item.isFavorite ? 'ci:heart-fill' : 'ci:heart-outline'"
              :width="18"
          /></i>
        </a-button>
        <!-- <a-button v-else id="RemoveFromFavBtn" type="default" class="px-5 py-3 uppercase font-bold" large>
          Remove from favorites <i class="anticon"><Icon icon="ci:heart-fill" :width="18" /></i>
        </a-button> -->
      </a-col>
    </a-row>
  </div>
</template>

<script>
import { Icon } from "@iconify/vue2";

import VPageHeader from "../components/VPageHeader.vue";

import api from "../api";
import router from "../router";
import store from "../store";

export default {
  data() {
    return {
      item: null,
      isInCart: false,
      accountId: store.state.id,
    };
  },
  mounted() {
    api
      .get(`/public/api/itemCatalog/${this.$route.params.itemId}`)
      .then((res) => {
        this.item = res.data;

        // if user is logged in, get favorite state and cart state
        if (this.accountId) {
          api
            .get(`/public/api/${this.accountId}/favorites/${this.item.id}`)
            .then((res) => {
              this.item.isFavorite = res.data.isFavorite;
            });

          api
            .get(`/public/api/${this.accountId}/cart/${this.item.id}`)
            .then((res) => {
              this.isInCart = res.data.isInCart;
            });
        }
      });
  },
  methods: {
    toggleFavorite() {
      // if user is not signed in, redirect
      if (this.accountId === null) {
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
    toggleCart() {
      // if user is not signed in, redirect
      if (this.accountId === null) {
        store.commit(
          "setWarningMsg",
          "The action you are trying to perform requires you to sign in first."
        );
        router.push("/signIn");

        return;
      }

      if (!this.isInCart) {
        api
          .post(`/public/api/${store.state.id}/cart`, {
            itemId: this.item.id,
          })
          .then(() => {
            console.log("added to cart");
            this.isInCart = true;
          });
      } else {
        api
          .delete(`/public/api/${store.state.id}/cart`, {
            data: { itemId: this.item.id },
          })
          .then(() => {
            console.log("removed from cart");
            this.isInCart = false;
          });
      }
    },
  },
  components: {
    Icon,
    VPageHeader,
  },
};
</script>

<style lang="postcss">
#AddToCartBtn {
  height: auto !important;

  &:hover i {
    @apply text-gray-600 !important;
  }

  & span {
    @apply relative !important;
    top: 3px;
  }

  & i {
    @apply text-green-400 !important;
    transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
  }
}

#RemoveFromCartBtn {
  height: auto !important;

  & span {
    @apply relative !important;
    top: 3px;
  }
}

#SaveToFavBtn {
  height: auto !important;

  &:hover {
    @apply border-gray-100 !important;
  }

  & span {
    @apply relative !important;
    top: 3px;
  }

  & i {
    transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
  }
  &:hover i {
    @apply text-red-400 !important;
  }
}

#RemoveFromFavBtn {
  height: auto !important;

  &:hover {
    @apply border-gray-100 !important;
  }

  & span {
    @apply relative !important;
    top: 3px;
  }

  & i {
    @apply text-red-400 !important;
    transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
  }
}

#SellerRating {
  @apply relative !important;
  top: 1px;
}
</style>
