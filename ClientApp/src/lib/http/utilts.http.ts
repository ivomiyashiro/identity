export const parseSuccessResponse = async <T>(
  response: Response
): Promise<T> => {
  const contentType = response.headers.get("content-type");

  if (
    contentType?.includes("application/json") ||
    contentType?.includes("application/problem+json")
  ) {
    return (await response.json()) as unknown as T;
  }

  if (contentType?.includes("text/")) {
    return (await response.text()) as unknown as T;
  }

  return (await response.blob()) as unknown as T;
};
