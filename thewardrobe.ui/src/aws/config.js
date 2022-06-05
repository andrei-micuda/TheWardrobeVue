const AWS_CONFIG = {
  BUCKET_NAME: "thewardrobe-media",
  BUCKET_REGION: "eu-central-1",
  IDENTITY_POOL_ID: "eu-central-1:2e1573bc-3d06-484c-aa38-214c25d961e7",
};

const RESOURCE_URL_TEMPLATE = (key) =>
  `https://${AWS_CONFIG.BUCKET_NAME}.s3.${AWS_CONFIG.BUCKET_REGION}.amazonaws.com/${key}`;

module.exports = {
  ...AWS_CONFIG,
  RESOURCE_URL_TEMPLATE,
};
