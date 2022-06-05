<template>
  <div class="text-center">
    <a-button type="primary" @click="toggleNewItemModal()" class="mb-10"
      >List Item</a-button
    >
    <a-modal
      id="TheSellingModal"
      v-model="showModal"
      title="List a new item"
      :footer="null"
      @ok="$emit('ok')"
      @cancel="$emit('cancel')"
    >
      <a-steps :current="current" labelPlacement="vertical">
        <a-step v-for="item in steps" :key="item.title" :title="item.title">
          <Icon :icon="item.icon" slot="icon" width="20" />
        </a-step>
      </a-steps>
      <div class="steps-content">
        <TheImageUploadStep
          v-if="current == 0"
          :handleUpload="handleUpload"
          :uploadedImages="uploadedImages"
        />
        <TheImageClassificationStep
          v-if="current == 1"
          :uploadedImages="uploadedImages"
          :predictedCategory="predictedCategory"
          :setPredictedCategory="setPredictedCategory"
        />
        <TheAddDetailsStep
          v-if="current == 2"
          :predictedCategory="predictedCategory"
          :handleUpload="handleUpload"
          :uploadedImages="uploadedImages"
          @refreshGrid="() => $emit('refreshGrid')"
          @toggleModal="toggleNewItemModal()"
        />
      </div>
      <div class="steps-action">
        <a-button
          v-if="current < steps.length - 1"
          type="primary"
          @click="next"
        >
          Next
        </a-button>
        <a-button
          v-if="current == steps.length - 1"
          type="primary"
          @click="handleDone"
        >
          Done
        </a-button>
        <a-button v-if="current > 0" style="margin-left: 8px" @click="prev">
          Previous
        </a-button>
      </div>
    </a-modal>
  </div>
</template>

<script>
import $ from "cash-dom";
// import VButton from '../VButton.vue';
import TheImageUploadStep from "./TheImageUploadStep.vue";
import TheImageClassificationStep from "./TheImageClassificationStep.vue";
import TheAddDetailsStep from "./TheAddDetailsStep.vue";
import { Icon } from "@iconify/vue2";

export default {
  data() {
    return {
      showModal: false,
      current: 0,
      steps: [
        {
          title: "Upload",
          icon: "clarity:image-gallery-solid",
        },
        {
          title: "Identify",
          icon: "carbon:ai-results",
        },
        {
          title: "Details",
          icon: "jam:write-f",
        },
      ],
      uploadedImages: [],
      predictedCategory: null,
    };
  },
  methods: {
    toggleNewItemModal() {
      this.showModal = !this.showModal;
    },
    next() {
      this.current++;

      // if no images have been uploaded and current step is classifciation, skip
      if (this.current === 1 && this.uploadedImages.length <= 0) {
        console.log("Skipping classification");
        this.current++;
      }
    },
    prev() {
      this.current--;
    },
    handleUpload(images) {
      this.uploadedImages = images;
    },
    setPredictedCategory(cat) {
      this.predictedCategory = cat;
    },
    handleDone() {
      $("#AddItemBtn").trigger("click");
    },
  },
  components: {
    //  VButton,
    Icon,
    TheImageUploadStep,
    TheImageClassificationStep,
    TheAddDetailsStep,
  },
};
</script>

<style lang="postcss">
#TheSellingModal {
  & .ant-modal-header {
    @apply bg-gray-600 border-b border-gray-700;
  }

  & .ant-modal-body {
    @apply bg-gray-500;

    /* Steps */
    & .ant-steps-item-icon {
      @apply py-2;
    }

    /* Current Step */
    & .ant-steps-item-process .ant-steps-item-icon {
      @apply bg-green-400 border-green-400 p-2;

      & .ant-steps-icon {
        @apply text-gray-800;
      }
    }

    & .ant-steps-item-process .ant-steps-item-content {
      & .ant-steps-item-title {
        @apply text-white;
      }
    }

    /* Done Steps */
    & .ant-steps-item-finish .ant-steps-icon {
      /* @apply text-gray-800; */
      @apply text-green-400;
    }

    & .ant-steps-item-finish .ant-steps-item-title::after {
      @apply bg-white;
    }
  }

  & .ant-modal-title,
  & .ant-modal-close-icon {
    @apply text-white;
  }

  /* Modal content */
  & .steps-content {
    @apply my-4;
  }
}
</style>
