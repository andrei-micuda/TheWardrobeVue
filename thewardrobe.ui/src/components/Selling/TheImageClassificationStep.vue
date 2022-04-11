<template>
  <div class="h-36">
    <!-- <lottie v-if="predictedCategory === null" :options="defaultOptions" :height="150" :width="150" /> -->
    <!-- <p v-else>{{predictedCategory}}</p> -->
    <lottie v-if="isPredicting" :options="defaultOptions" :height="150" :width="150" />
    <div v-else class="text-center flex flex-col h-full">
      <h2 class="text-gray-100">The predicted category based on your images is:</h2>
      <div class="flex-1 flex flex-col justify-center">
        <a-tooltip id="PredictedCategory" class="cursor-default mx-auto inline-block rounded-md uppercase text-white border-2 border-gray-600 px-4 py-2">
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
            <a-tooltip class="cursor-default mx-auto inline-block rounded-md uppercase text-white border border-gray-600 px-2 py-1">
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
  // AWS imports
  import { CognitoIdentityClient } from "@aws-sdk/client-cognito-identity";
  import {
      fromCognitoIdentityPool,
  } from "@aws-sdk/credential-provider-cognito-identity";
  import { S3Client, PutObjectCommand } from "@aws-sdk/client-s3";

  import { BUCKET_NAME, BUCKET_REGION, IDENTITY_POOL_ID, RESOURCE_URL_TEMPLATE } from "../../config/aws.config.js";

  import Lottie from 'vue-lottie';
  import axios from 'axios';
  import animationData from '../../assets/image-scan.json'

  const initAwsS3 = () => {
    return new S3Client({
      region: BUCKET_REGION,
      credentials: fromCognitoIdentityPool({
        client: new CognitoIdentityClient({ region: BUCKET_REGION }),
        identityPoolId: IDENTITY_POOL_ID
      })
    });
  }

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
        
        let res = await axios.post("/api/clothesClassification", reqBody);
        
        this.predictions = res.data.predictions.map(pred => {
          pred[1] = `${pred[1].toFixed(1)}%`;
          return pred;
        });
        this.isPredicting = false;
        this.setPredictedCategory(this.predictions[0][0]);
      },
      async uploadToBucket(s3Client, file) {
        
        const uploadParams = {
          Bucket: BUCKET_NAME,
          // Add the required 'Key' parameter using the 'path' module.
          Key: file.uploadName,
          // Add the required 'Body' parameter
          Body: file.originFileObj,
          ContentType: "image/png"
        };

        let resourceUrl = null;
        try {
          const data = await s3Client.send(new PutObjectCommand(uploadParams));

          console.log("Success", data)
          resourceUrl = RESOURCE_URL_TEMPLATE(uploadParams.Key);
        } catch (err) {
          console.log("Error", err);
        }
        return resourceUrl;
      },
      async uploadFilesToAws() {
        const s3Client = initAwsS3();
        await Promise.all(this.uploadedImages.map(async file => {
          const resourceUrl = await this.uploadToBucket(s3Client, file);
          if(resourceUrl !== null) {
            file.resourceUrl = resourceUrl;
          }
        }));
      }
    },
    mounted() {
      this.uploadFilesToAws()
        .then(() => {
          console.log("Done uploading");
          console.log(this.predictions)

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