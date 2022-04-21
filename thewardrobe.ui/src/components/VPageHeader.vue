<template>
  <div class="w-screen flex items-center p-5 justify-between">
    <div class="flex items-center">
      <button @click="$emit('back')">
        <Icon icon="eva:arrow-back-outline" width="32" height="32"
        class="mr-5 cursor-pointer rounded-full hover:bg-gray-800" />
      </button>
      <span class="inline-flex items-center text-xl relative top-0.5">{{title}}</span>
    </div>
    
    <div v-if="showEditActions" class="flex flex-row-reverse items-center">
      <a-button size="large" type="primary" class="ml-5" @click="$emit('apply')">Apply</a-button>
      <!-- <VButton @click="$emit('apply')">Apply</VButton> -->
      <a-button size="large" type="danger" ghost @click="showDeleteConfirm">
        Delete
      </a-button>
    </div>
  </div>
</template>

<script>
  import { Icon } from '@iconify/vue2';
  // import VButton from './VButton.vue';
  export default {
    props: {
      title: {
        type: String,
      },
      showEditActions: {
        type: Boolean,
        default: false,
      }
    },
    methods: {
      showDeleteConfirm() {
        var that = this;
        this.$confirm({
          title: 'Are you sure you want to delete this item?',
          content: 'This action is ireversible.',
          okText: 'Yes',
          okType: 'danger',
          cancelText: 'No',
          onOk() {
            that.$emit('delete');
          },
          // onCancel() {
          //   console.log('Cancel');
          // },
        });
      },
    },
    components: {
      Icon,
      // VButton
    },
  }
</script>

<style lang="postcss">

</style>