<template>
  <div>
    <a-upload
      :class="size"
      accept="image/*"
      :before-upload="() => false"
      list-type="picture-card"
      :file-list="fileList"
      @preview="handlePreview"
      @change="handleChange"
    >
      <div v-if="fileList.length < 8">
        <a-icon type="plus" />
        <div v-if="size !== 'small'" class="ant-upload-text">Upload</div>
      </div>
    </a-upload>
    <a-modal :visible="previewVisible" :footer="null" @cancel="handleCancel">
      <img alt="example" style="width: 100%" :src="previewImage" />
    </a-modal>
  </div>
</template>
<script>
function getBase64(file) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });
}
export default {
  props: {
    handleUpload: {
      type: Function,
    },
    uploadedImages: {
      type: Array,
    },
    size: {
      type: String,
      default: "default",
    },
  },
  data() {
    return {
      previewVisible: false,
      previewImage: "",
      fileList: [],
    };
  },
  mounted() {
    this.fileList = this.uploadedImages;
  },
  methods: {
    handleCancel() {
      this.previewVisible = false;
    },
    async handlePreview(file) {
      if (!file.url && !file.preview) {
        file.preview = await getBase64(file.originFileObj);
      }
      this.previewImage = file.url || file.preview;
      this.previewVisible = true;
    },
    handleChange({ fileList }) {
      this.fileList = fileList;
      this.handleUpload(fileList);
    },
  },
};
</script>
<style lang="postcss">
.ant-upload-select-picture-card {
  @apply bg-transparent hover:border-green-400 !important;

  & i {
    @apply text-white;
    font-size: 32px;
  }

  & .ant-upload-text {
    @apply text-white;
    margin-top: 8px;
  }
}

.small {
  & .ant-upload-select-picture-card {
    @apply w-10 h-10 !important;
    & i {
      font-size: 24px !important;
    }
  }

  & .ant-upload-list-item,
  & .ant-upload-list-picture-card-container {
    @apply w-10 h-10 p-0 !important;
  }
}

/* .ant-upload-list-picture-card .ant-upload-list-item {
  width: 100% !important;
  height: 100% !important;
} */
</style>
