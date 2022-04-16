<template>
  <li>
    <router-link :to="itemLink">
      <div class="card w-full rounded bg-gray-800
                  transition ease-in-out">
        <a-carousel arrows>
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

        <a-row class="p-4">
          <a-col :span="16" class="text-left">
            <p class="price text-gray-100 text-lg">{{item.price}} RON</p>
            <p class="text-gray-200">Size: {{item.size}}</p>
            <p class="text-gray-200">Brand: {{item.brand}}</p>
          </a-col>
          <!-- <a-col :span="8">
            <router-link :to="{name: 'editItem', params: {itemId: item.id}}"><VButton>Edit</VButton></router-link>
          </a-col> -->
        </a-row>
      </div>
    </router-link>
  </li>
</template>

<script>
  import { Icon } from '@iconify/vue2';

  // import VButton from "../VButton";

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
        if(this.editable)
          return {name: 'editItem', params: {itemId: this.item.id}}
        
        return {name: 'viewItem', params: {itemId: this.item.id}}
      }
    },
    components: {
      Icon,
      // VButton
    },
  }
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
  font-family: 'Libre Franklin', sans-serif;
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