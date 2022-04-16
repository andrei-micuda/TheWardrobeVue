<template>
  <a-form :form="form" v-bind="formItemLayout" @submit="handleSubmit" >
  <VPageHeader @back="$router.back()" @apply="handleApply" @delete="handleDelete" title="Edit Item" />
    <div class="w-8/12 mx-auto bg-gray-800 px-10 py-8">
      <a-form-item label="Product Name">
        <a-input
          v-decorator="[
            'productName',
            {
              rules: [
                {
                  required: true,
                  message: 'Please provide a product name!',
                },
              ],
            },
          ]"
        />
      </a-form-item>
      <a-form-item label="Price" name="price">
        <a-input
          suffix="RON"
          v-decorator="[
            'price',
            {
              rules: [
                {
                  required: true,
                  message: 'Please provide a price!',
                },
              ],
            },
          ]"
        />
      </a-form-item>
      <a-form-item label="Gender">
        <a-radio-group class="flex" v-decorator="[
          'gender',
          {
            initialValue: 'unisex',
            rules: [
              {
                required: true,
                message: 'Please select a gender.',
              }
            ]
          }
          ]" button-style="solid">
          <a-radio-button value="unisex">
            Unisex
          </a-radio-button>
          <a-radio-button value="male">
            Male
          </a-radio-button>
          <a-radio-button value="female">
            Female
          </a-radio-button>
        </a-radio-group>
      </a-form-item>
      <a-form-item label="Category">
        <a-tree-select
          v-decorator="[
          'category',
          {
            initialValue: 'Tee',
            rules: [
              {
                required: true,
                message: 'Please select a category.',
              }
            ]
          }
          ]"
          @change="val => {selectedCategory = val;}"
          style="width: 100%"
          :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
          :tree-data="categoryData"
          show-search
          allow-clear
          placeholder="Please select a category"
        >
        </a-tree-select>
      </a-form-item>
      <a-form-item label="Brand">
        <a-select
          v-decorator="[
          'brand',
          {
            rules: [
              {
                required: true,
                message: 'Please select a brand.',
              }
            ]
          }
          ]"
          @change="val => {selectedBrand = val;}"
          style="width: 100%"
          :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
          :options="brandData"
          placeholder="Please select a brand"
          show-search
          allow-clear
        >
        </a-select>
      </a-form-item>
      <a-form-item label="Size">
        <a-select
          v-decorator="[
          'size',
          {
            rules: [
              {
                required: true,
                message: 'Please select a size.',
              }
            ]
          }
          ]"
          @change="val => {selectedSize = val;}"
          style="width: 100%"
          :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
          :options="sizeData"
          placeholder="Please select a size"
          show-search
          allow-clear
        >
        </a-select>
      </a-form-item>
      <a-form-item label="Images" name="images" class="mb-0">
        <a-upload
            list-type="picture"
            :file-list="images"
            :before-upload="() => false"
            :remove="handleRemove"
            @change="handleChange"
          >
            <a-button> <a-icon type="upload" />Upload</a-button>
          </a-upload>
      </a-form-item>
      <a-button id="UpdateItemBtn" class="hidden" type="primary" html-type="submit">
        Submit
      </a-button>
    </div>
  </a-form>
</template>

<script>
  /* eslint-disable no-unused-vars */
  // import $ from "cash-dom";
  import { v4 as uuidv4 } from 'uuid';
  import $ from "cash-dom";

  import VPageHeader from "../components/VPageHeader.vue"
  
  import constData from '../const';
  import api from "../api";
  import getS3Client from "../aws";
  import router from "../router";
  import canEdit from "../router/guards/canEdit";
  import notifier from "../notifier";
  // import store from '../store';

  const categories = {
    clothing: constData.clothingCategories
  };

  const categoryData = [
    {
      title: 'Clothing',
      value: 'Clothing',
      key: 'Clothing',
      selectable: false,
      children: [
        ...categories["clothing"].map(cat => {
          return {title: cat, value: cat, key: cat}
          })
      ],
    },
    {
      title: 'Footwear',
      value: 'Footwear',
      key: 'Footwear'
    },
    {
      title: "Accessories",
      value: "Accessories",
      key: "Accessories"
    }
  ];

  const sizes = {
    "clothing&acc": [...constData.clothingSizes.map(cat => {
          return {title: cat, value: cat, key: cat}
          })
    ],
    "footwear": [...constData.footwearSizes.map(cat => {
        return {title: cat, value: cat, key: cat}
        })
    ],
  }

  const brandData = [...constData.brands.map(brand => {
    return {title: brand, value: brand}
  })];

  export default {
    data() {
      return {
        images: null,
        categoryData,
        brandData,
        selectedCategory: null,
        selectedSize: null,
        selectedBrand: null,
        formItemLayout: {
          labelCol: { span: 6 },
          wrapperCol: { span: 18 },
        },
      }
    },
    beforeRouteEnter(to, from, next) {
      canEdit(to.params.itemId, next);
    },
    beforeRouteUpdate(to, from, next) {
      canEdit(to.params.itemId, next);
    },
    beforeCreate() {
      this.form = this.$form.createForm(this, { name: 'updateItem' });
    },
    mounted() {
      api.get(`/api/itemCatalog/${this.$route.params.itemId}`)
      .then(res => {
        var { productName, price, gender, category, size, brand, images } = res.data;

        this.form.setFieldsValue({ productName, price, gender, category, size, brand });
        this.images = images.map((url, i) => {
          return {
            uid: i,
            name: `Image ${i + 1}`,
            url,
            thumbUrl: url}
        });
      });
    },
    methods: {
      handleChange({ fileList }) {
        console.log(fileList)
        this.images = fileList.map((file, i) => {
          file.initialName = file.name;
          file.name = `Image ${i+1}`;
          return file;
        });
      },
      handleRemove(file) {
        console.log(file)
      },
      handleApply() {
        $("#UpdateItemBtn").trigger("click");
      },
      handleDelete() {
        api.delete(`/api/itemCatalog/${this.$route.params.itemId}`)
          .then(() => {
            notifier.success("Item has been delete successfully.");
            router.push('/sell');
          })
          .catch(error => {
            console.error(error);
            error.handleGlobally && error.handleGlobally();
          });
      },
      async handleSubmit(e) {
        e.preventDefault();

        // Uploading new Images
        var s3Client = getS3Client();

        await Promise.all(this.images.map(async image => {
          if(image.url === undefined) {
            const uuid = uuidv4();
            const fileExtention = image.initialName.split('.').pop();
            image.uploadName = `${uuid}.${fileExtention}`;
            image.url = await s3Client.uploadFileToBucket(image);
          }
        }));

        console.log('Submitting...')
        this.form.validateFields(async (err, values) => {
          if (!err) {
            var imagesUrls = this.images.map(img => img.url);

            await api.put(`/api/itemCatalog/${this.$route.params.itemId}`, {
              ...values,
              images: imagesUrls})
              .then(function() {
                notifier.success("Changes have been saved");
                router.push('/sell');
              })
              .catch(error => {
                console.error(error);
                error.handleGlobally && error.handleGlobally();
              });
          }
        });
      }
    },
    computed: {
      isClothing() {
        return categories["clothing"].includes(this.selectedCategory);
      },
      isFootwear() {
        return (this.selectedCategory === "Footwear");
      },
      isAccessory() {
        return (this.selectedCategory === "Accessories");
      },
      sizeData() {
        if(this.isClothing || this.isAccessory) {
          return sizes["clothing&acc"];
        }
        else {
          return sizes["footwear"];
        }
      }
    },
    components: {
      VPageHeader,
    },
  }
</script>

<style lang="postcss">

</style>