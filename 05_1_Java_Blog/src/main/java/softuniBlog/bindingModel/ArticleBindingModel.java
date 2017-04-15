package softuniBlog.bindingModel;

import javax.validation.constraints.NotNull;

public class ArticleBindingModel {
    @NotNull
    private String title;

    @NotNull
    private String Content;

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getContent() {
        return Content;
    }

    public void setContent(String content) {
        Content = content;
    }
}
