<template>
  <div class="h-36">
    <!-- <lottie v-if="predictedCategory === null" :options="defaultOptions" :height="150" :width="150" /> -->
    <!-- <p v-else>{{predictedCategory}}</p> -->
    <lottie v-if="predictedCategory === null" :options="defaultOptions" :height="150" :width="150" />
    <div v-else class="text-center flex flex-col h-full">
      <h2 class="text-gray-100">The predicted category based on your images is:</h2>
      <div class="flex-1 flex flex-col justify-center">
        <div id="PredictedCategory" class="mx-auto inline-block rounded-md uppercase text-white border-2 border-gray-600 px-4 py-2">{{predictedCategory}}</div>
      </div>
      <p class="text-gray-100 opacity-60 text-xs">Don't worry, you can change the category in the next step.</p>
    </div>
  </div>
</template>

<script>
  import Lottie from 'vue-lottie';
  import axios from 'axios';
  import animationData from '../../assets/image-scan.json'

  export default {
    data() {
      return {
        defaultOptions: {animationData: animationData}
      }
    },
    props: {
      uploadedImages: {
        type: Array
      },
      predictedCategory: {
        type: String
      },
      setPredictedCategory: {
        type: Function
      }
    },
    methods: {
      async initializeClassification() {
        let category = "Sweater";
        let formData = new FormData();
        this.uploadedImages.forEach(file => {
          formData.append("files", file.originFileObj);
        });
        let res = await axios.post("/api/clothesClassification", formData);
        console.log(res);
        // await new Promise((resolve) => setTimeout(resolve, 2000));
        return category;
      }
    },
    mounted() {
      this.initializeClassification().then((val) => this.setPredictedCategory(val));
    },
    components: {
      Lottie,
    },
  }
</script>

<style lang="postcss">
#PredictedCategory {
  font-family: 'Josefin Sans', sans-serif;
}