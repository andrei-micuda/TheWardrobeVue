import { CognitoIdentityClient } from "@aws-sdk/client-cognito-identity";
import { fromCognitoIdentityPool } from "@aws-sdk/credential-provider-cognito-identity";
import { S3Client, PutObjectCommand } from "@aws-sdk/client-s3";

import {
  BUCKET_NAME,
  BUCKET_REGION,
  IDENTITY_POOL_ID,
  RESOURCE_URL_TEMPLATE,
} from "./config.js";

S3Client.prototype.uploadFileToBucket = async function (file) {
  const uploadParams = {
    Bucket: BUCKET_NAME,
    // Add the required 'Key' parameter using the 'path' module.
    Key: file.uploadName,
    // Add the required 'Body' parameter
    Body: file.originFileObj,
    ContentType: "image/png",
  };

  let resourceUrl = null;
  try {
    const data = await this.send(new PutObjectCommand(uploadParams));

    console.log("Success", data);
    resourceUrl = RESOURCE_URL_TEMPLATE(uploadParams.Key);
  } catch (err) {
    console.log("Error", err);
  }
  return resourceUrl;
};

const getS3Client = () => {
  var client = new S3Client({
    region: BUCKET_REGION,
    credentials: fromCognitoIdentityPool({
      client: new CognitoIdentityClient({ region: BUCKET_REGION }),
      identityPoolId: IDENTITY_POOL_ID,
    }),
  });

  return client;
};

export default getS3Client;
