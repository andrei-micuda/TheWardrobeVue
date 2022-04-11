<template>
  <div>
    <a-form :form="form" v-bind="formItemLayout" @submit="handleSubmit">
      <TheInlineImageUpload size="small" :handleUpload="handleUpload" :uploadedImages="uploadedImages" />
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
      <a-form-item label="Price">
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
        <a-radio-group v-decorator="[
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
            initialValue: predictedCategory,
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
      <a-button id="AddItemBtn" class="hidden" type="primary" html-type="submit">
        Submit
      </a-button>
    </a-form>
  </div>
</template>

<script>
  import api from "../../api";
  import notifier from "../../notifier";
  import store from '../../store';

  import TheInlineImageUpload from './TheInlineImageUpload.vue';

  const categories = {
    clothing: ["Anorak", "Blazer", "Blouse", "Bomber", "Button-Down", "Cardigan", "Flannel", "Halter", "Henley", "Hoodie", "Jacket", "Jersey", "Parka", "Peacoat", "Poncho", "Sweater", "Tank", "Tee", "Top", "Turtleneck", "Capris", "Chinos", "Culottes", "Cutoffs", "Gauchos", "Jeans", "Jeggings", "Jodhpurs", "Joggers", "Leggings", "Sarong", "Shorts", "Skirt", "Sweatpants", "Sweatshorts", "Trunks", "Caftan", "Cape", "Coat", "Coverup", "Dress", "Jumpsuit", "Kaftan", "Kimono", "Nightdress", "Onesie", "Robe", "Romper", "Shirtdress", "Sundress"]
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
    "clothing&acc": [...["XS", "S", "M", "L", "XL", "one-size"].map(cat => {
          return {title: cat, value: cat, key: cat}
          })
    ],
    "footwear": [...["37", "38", "39", "40", "41", "42", "43", "44", "45", "46"].map(cat => {
        return {title: cat, value: cat, key: cat}
        })
    ],
  }

  const brandData = [...['Nike', 'Adidas', 'Zara', 'H&M', 'C&A', 'Pull&Bear', 'Collin\'s', 'Stradivarius', 'U.S. Polo Assn.', 'Tommy Hilfiger', 'Champion', 'The North Face', 'Hugo Boss', 'Superdry', 'Lacoste', 'New Balance'].map(brand => {
    return {title: brand, value: brand}
  })];

  export default {
    props: {
      predictedCategory: {
        type: String,
      },
      handleUpload: {
        type: Function
      },
      uploadedImages: {
        type: Array
      },
      toggleNewItemModal: {
        type: Function
      }
    },
    data() {
      return {
        categoryData,
        brandData,
        selectedCategory: null,
        selectedSize: null,
        selectedGender: "unisex",
        selectedBrand: null,
        formItemLayout: {
          labelCol: { span: 6 },
          wrapperCol: { span: 18 },
        },
      }
    },
    methods: {
      async handleSubmit(e) {
        e.preventDefault();
        this.form.validateFields(async (err, values) => {
          if (!err) {
            var uploadedImagesUrls = this.uploadedImages.map(img => img.resourceUrl);

            var that = this;
            await api.post("/api/itemCatalog", {
              ...values,
              images: uploadedImagesUrls,
              sellerId: store.state.id})
              .then(function(res) {
                var item = res.data;
                item.images = that.uploadedImages;
                console.log(item)
                notifier.success("Successfully listed product.");
                that.$emit("toggleModal");
                that.$emit("refreshGrid");
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
    beforeCreate() {
      this.form = this.$form.createForm(this, { name: 'addItem' });
    },
    mounted() {
      this.selectedCategory = this.predictedCategory;
    },
    components: {
      TheInlineImageUpload,
    },
  }
</script>

<style lang="postcss">

</style>