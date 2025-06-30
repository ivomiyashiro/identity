import { Button } from "@/components/ui/button";
import { useLogoutMutation } from "../hooks/use-logout.mutation";
import { PageLayout } from "@/components/layouts/page.layout";

const PrivatePage = () => {
  const { mutate: logout, isPending } = useLogoutMutation();

  return (
    <PageLayout title="Private Page">
      <Button onClick={() => logout()} disabled={isPending}>
        {isPending ? "Logging out..." : "Logout"}
      </Button>
    </PageLayout>
  );
};

export default PrivatePage;
