<template>
  <ul class="bg-gray-700 p-4">
    <li class="item" v-for="item in items" :key="item.id">
      <a-row type="flex">
        <a-col :span="5">
          <img :src="item.images[0]" class="w-full" />
        </a-col>
        <a-col :span="10" class="px-4 flex items-center md:items-start">
          <p class="font-semibold text-lg">{{item.productName}}</p>
        </a-col>
        <a-col :span="9" class="flex flex-col justify-center md:justify-start">
          <button v-if="isDeletable" class="self-end" @click="$emit('deleteItem', item.id)">
            <Icon icon="charm:cross" height="36" />
          </button>
          <p class="text-right text-xl">{{item.price}} RON</p>
        </a-col>
      </a-row>
    </li>
  </ul>
</template>

<script>
  import { Icon } from "@iconify/vue2";

  export default {
    props: {
      items: {
        type: Array,
      },
      isDeletable: {
        type: Boolean,
        default: false
      }
    },
    components: {
      Icon,
    },
  }
</script>

<style lang="postcss" scoped>
.item:not(:last-of-type) {
    @apply border-b-2 border-gray-600 mb-4 pb-4 !important;
  }
</style>