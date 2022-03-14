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
  // AWS imports
  import { CognitoIdentityClient } from "@aws-sdk/client-cognito-identity";
  import {
      fromCognitoIdentityPool,
  } from "@aws-sdk/credential-provider-cognito-identity";
  import { S3Client, PutObjectCommand } from "@aws-sdk/client-s3";


  import Lottie from 'vue-lottie';
  import axios from 'axios';
  import animationData from '../../assets/image-scan.json'

  const initAwsS3 = () => {
    const bucketRegion = "eu-central-1";
    const IdentityPoolId = "eu-central-1:2e1573bc-3d06-484c-aa38-214c25d961e7";

    return new S3Client({
      region: bucketRegion,
    credentials: fromCognitoIdentityPool({
      client: new CognitoIdentityClient({ region: bucketRegion }),
        identityPoolId: IdentityPoolId
      })
    });
  }

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
      },
      async uploadToBucket(s3Client) {
        const bucketName = "thewardrobe-media";

        const uploadParams = {
          Bucket: bucketName,
          // Add the required 'Key' parameter using the 'path' module.
          Key: "test.png",
          // Add the required 'Body' parameter
          Body: this.uploadedImages[0].originFileObj,
        };

        try {
          const data = await s3Client.send(new PutObjectCommand(uploadParams));
          console.log("Success", data);
          return data; // For unit tests.
        } catch (err) {
          console.log("Error", err);
        }
      }
    },
    mounted() {
      const s3Client = initAwsS3();
      this.uploadToBucket(s3Client);
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