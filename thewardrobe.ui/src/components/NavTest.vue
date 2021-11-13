<template>
  <div class="hidden xl:flex items-center space-x-5 mb-4">
              <a class="hover:text-gray-200" href="#">
                <Icon :icon="heartIcon" class="h-6 w-6" />
              </a>
              <a class="flex items-center hover:text-gray-200" href="#">
                  <Icon :icon="cartIcon" class="h-6 w-6" />
                <span class="flex absolute -mt-5 ml-4">
                  <span class="animate-ping absolute inline-flex h-3 w-3 rounded-full bg-pink-400 opacity-75"></span>
                    <span class="relative inline-flex rounded-full h-3 w-3 bg-pink-500">
                    </span>
                  </span>
              </a>
              <!-- Sign In / Register      -->
              <a class="flex items-center hover:text-gray-200" href="#">
                  <Icon :icon="profileIcon" class="h-6 w-6" />
              </a>
              
            </div>
</template>

<script>
import { Icon } from '@iconify/vue2';

  export default {
    props: {
      cartIcon: {
        type: String
      },
      heartIcon: {
        type: String
      },
      profileIcon: {
        type: String
      },
    },
    components: {
      Icon,
    },
  }
</script>

<style scoped>

</style>