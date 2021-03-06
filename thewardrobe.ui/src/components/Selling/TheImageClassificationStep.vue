<template>
  <div class="h-36">
    <!-- <lottie v-if="predictedCategory === null" :options="defaultOptions" :height="150" :width="150" /> -->
    <!-- <p v-else>{{predictedCategory}}</p> -->
    <lottie v-if="isPredicting" :options="defaultOptions" :height="150" :width="150" />
    <div v-else class="text-center flex flex-col h-full">
      <h2 class="text-gray-100">The predicted category based on your images is:</h2>
      <div class="flex-1 flex flex-col justify-center">
        <a-tooltip id="PredictedCategory" class="cursor-default mx-auto inline-block rounded-md uppercase text-white border-2 border-gray-700 px-4 py-1">
          <template slot="title">
            {{predictions[0][1]}}
          </template>
          {{predictions[0][0]}}
        </a-tooltip>
      </div>
      <div class="text-gray-100 text-xs mb-2">
        <p>Other predictions: </p>
        <ul class="space-x-2 mt-2">
          <li v-for="pred in predictions.slice(1)" :key="pred[0]" class="inline">
            <a-tooltip class="cursor-default mx-auto inline-block rounded-md uppercase text-white border border-gray-700 px-2 py-1">
              <template slot="title">
                {{pred[1]}}
              </template>
              {{pred[0]}}
            </a-tooltip>
          </li>
        </ul>
      </div>
      <p class="text-gray-100 opacity-60 text-xs">Don't worry, you can change the category in the next step.</p>
    </div>
  </div>
</template>

<script>
  import Lottie from 'vue-lottie';
  import api from '../../api';
  import animationData from '../../assets/image-scan.json'

  export default {
    data() {
      return {
        defaultOptions: {animationData: animationData},
        isPredicting: {
          type: Boolean,
          default: true
        },
        predictions: {
          type: Array,
        },
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
        let reqBody = {
          "resourceUrl": this.uploadedImages[0].resourceUrl,
          "minScore": 70
        };
        // this.uploadedImages.forEach(file => {
        //   formData.append("files", file.originFileObj);
        // });
        
        let res = await api.post("/public/api/clothesClassification", reqBody);
        
        this.predictions = res.data.predictions.map(pred => {
          pred[1] = `${pred[1].toFixed(1)}%`;
          return pred;
        });
        this.isPredicting = false;
        this.setPredictedCategory(this.predictions[0][0]);
      },
      async uploadFilesToAws() {
        await Promise.all(
          this.uploadedImages.map(async (image) => {
            let formData = new FormData();
            formData.set("file", image.originFileObj);
            formData.set("uploadName", image.uploadName);

            const res = await api.post(`/public/api/imageUpload`, formData, {
              headers: {
                "content-type": "multipart/form-data",
              },
            });

            image.resourceUrl = res.data;
          })
        );
      }
    },
    mounted() {
      this.uploadFilesToAws()
        .then(() => {
          this.initializeClassification();
        });
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