<template>
  <div> 
    <VPageHeader @back="$router.back()" title="View Item" />
    <a-row class="w-8/12 mx-auto px-10 py-8">
      <a-col :span="12">
        <a-carousel arrows class="bg-gray-800">
          <template #prevArrow>
            <div class="custom-slick-arrow" style="left: 10px; z-index: 10">
              <Icon icon="bi:arrow-left-circle-fill" width="20" height="20" class="text-gray-600" />
            </div>
          </template>
          <template #nextArrow>
            <div class="custom-slick-arrow" style="right: 10px">
              <Icon icon="bi:arrow-right-circle-fill" width="20" height="20" class="text-gray-600" />
            </div>
          </template>
          <div v-for="url in item.images" :key="url">
            <img :src="url" alt="" class="mx-auto w-100">
          </div>
        </a-carousel>
      </a-col>

      <a-col :span="12" class="pl-6">
        <p class="text-2xl font-bold">{{item.productName}}</p>
        <p class="text-lg">Brand: {{item.brand}}</p>
        <p class="text-lg">Category: {{item.category}}</p>
        <p class="text-lg">Gender: {{item.gender}}</p>
        <p class="text-lg">Size: {{item.size}}</p>
        <p class="text-2xl font-bold">{{item.price}} RON</p>

        <a-button id="AddToCartBtn" type="primary" class="px-5 py-3 uppercase my-4 font-bold" large>
          Add to cart <i class="anticon"><Icon icon="eva:shopping-cart-outline" :width="20" /></i>
        </a-button>
        <br />
        <a-button id="SaveToFavBtn" type="default" class="px-5 py-3 uppercase font-bold" large>
          Save to favorites <i class="anticon"><Icon icon="feather:heart" :width="18" /></i>
        </a-button>
      </a-col>
    </a-row>
  </div>
</template>

<script>
  import { Icon } from '@iconify/vue2';

  import VPageHeader from "../components/VPageHeader.vue";

  import api from '../api';

  export default {
    data() {
      return {
        item: null
      }
    },
    mounted () {
      api.get(`/api/itemCatalog/${this.$route.params.itemId}`)
        .then(res => {
          console.log(res.data)
          this.item = res.data;
          // var { productName, price, gender, category, size, brand, images } = res.data;
        });
    },
    components: {
      Icon,
      VPageHeader
    },
  }
</script>

<style lang="postcss">
  #AddToCartBtn {
    height: auto !important;

    &:hover i {
      @apply text-gray-100 !important;
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
</style>