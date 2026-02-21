<template>
  <div class="post">
    <p>{{ item.title }}</p>
    <div>{{ item.body }}</div>
    <div class="post-details">
      <small>{{ item.id }}</small> |
      <small>{{ item.userId }}</small>
    </div>
  </div>
</template>

<script setup lang="ts">
import Post from "@/types/Post";

interface Props {
  item: Post;
}

const { item } = defineProps<Props>();
</script>

<style scoped>
.post {
  border-bottom: 1px solid #f5f5f5;
  margin-bottom: 1rem;
}

.post-details {
    display: flex;
}

</style>
